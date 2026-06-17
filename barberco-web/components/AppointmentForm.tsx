"use client";

import { useActionState, useEffect, useState } from "react";
import {
  initialAppointmentFormState,
  type AppointmentFormState,
} from "@/lib/appointment-form-state";
import { formatPrice } from "@/lib/format-price";
import { formatPhoneNumber } from "@/lib/format-phone";
import type { Service, TimeSlot } from "@/lib/types";
import { submitAppointment } from "@/app/appointment/actions";

function formatDateInput(date: Date): string {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const day = String(date.getDate()).padStart(2, "0");
  return `${year}-${month}-${day}`;
}

const inputClassName =
  "w-full border border-foreground/15 bg-background px-4 py-3 text-foreground outline-none transition-colors focus:border-accent";

type AppointmentFormProps = {
  services: Service[];
};

export function AppointmentForm({ services }: AppointmentFormProps) {
  const [state, formAction, isPending] = useActionState<
    AppointmentFormState,
    FormData
  >(submitAppointment, initialAppointmentFormState);

  const [phone, setPhone] = useState("");
  const [selectedIds, setSelectedIds] = useState<number[]>([]);
  const [date, setDate] = useState(() => formatDateInput(new Date()));
  const [times, setTimes] = useState<TimeSlot[]>([]);
  const [time, setTime] = useState("");
  const [loadingTimes, setLoadingTimes] = useState(true);

  useEffect(() => {
    let cancelled = false;

    async function loadTimes() {
      setLoadingTimes(true);
      setTime("");

      try {
        const response = await fetch(`/api/appointment/times?date=${date}`);
        const data = (await response.json()) as { times?: TimeSlot[] };
        if (!cancelled) {
          setTimes(data.times ?? []);
        }
      } catch {
        if (!cancelled) {
          setTimes([]);
        }
      } finally {
        if (!cancelled) {
          setLoadingTimes(false);
        }
      }
    }

    loadTimes();

    return () => {
      cancelled = true;
    };
  }, [date]);

  function toggleService(id: number) {
    setSelectedIds((current) =>
      current.includes(id)
        ? current.filter((serviceId) => serviceId !== id)
        : [...current, id],
    );
  }

  return (
    <form action={formAction} className="mt-8 space-y-6">
      {state.error && (
        <div
          className="border border-red-400/40 bg-red-950/20 px-4 py-3 text-sm text-red-300"
          role="alert"
        >
          {state.error}
        </div>
      )}

      <div className="grid gap-6 sm:grid-cols-2">
        <div>
          <label htmlFor="name" className="mb-2 block text-sm text-muted">
            Name
          </label>
          <input
            id="name"
            name="name"
            type="text"
            required
            autoComplete="name"
            className={inputClassName}
          />
        </div>

        <div>
          <label htmlFor="phone" className="mb-2 block text-sm text-muted">
            Phone
          </label>
          <input
            id="phone"
            name="phone"
            type="tel"
            required
            value={phone}
            onChange={(event) =>
              setPhone(formatPhoneNumber(event.target.value))
            }
            placeholder="(123) 456-7890"
            maxLength={14}
            autoComplete="tel"
            className={inputClassName}
          />
        </div>
      </div>

      <div className="grid gap-6 sm:grid-cols-2">
        <div>
          <label htmlFor="date" className="mb-2 block text-sm text-muted">
            Date
          </label>
          <input
            id="date"
            name="date"
            type="date"
            required
            value={date}
            min={formatDateInput(new Date())}
            onChange={(event) => setDate(event.target.value)}
            className={`${inputClassName} [color-scheme:dark]`}
          />
        </div>

        <div>
          <label htmlFor="time" className="mb-2 block text-sm text-muted">
            Time
          </label>
          <select
            id="time"
            name="time"
            required
            value={time}
            disabled={loadingTimes || times.length === 0}
            onChange={(event) => setTime(event.target.value)}
            className={inputClassName}
          >
            <option value="">
              {loadingTimes
                ? "Loading times..."
                : times.length === 0
                  ? "No availability"
                  : "Select a time"}
            </option>
            {times.map((slot) => (
              <option key={slot.value} value={slot.value}>
                {slot.display}
              </option>
            ))}
          </select>
        </div>
      </div>

      <div>
        <p className="mb-3 text-sm text-muted">Services</p>
        <div className="grid gap-2 sm:grid-cols-2">
          {services.map((service) => {
            const selected = selectedIds.includes(service.id);

            return (
              <button
                key={service.id}
                type="button"
                onClick={() => toggleService(service.id)}
                className={`flex items-center justify-between border px-4 py-3 text-left text-sm transition-colors ${
                  selected
                    ? "border-accent bg-accent/10 text-foreground"
                    : "border-foreground/15 text-muted hover:border-foreground/30 hover:text-foreground"
                }`}
              >
                <span>{service.name}</span>
                <span className="tabular-nums text-accent">
                  {formatPrice(service.price)}
                </span>
              </button>
            );
          })}
        </div>
        {selectedIds.map((id) => (
          <input key={id} type="hidden" name="serviceIds" value={id} />
        ))}
      </div>

      <button
        type="submit"
        disabled={isPending || selectedIds.length === 0}
        className="w-full border border-accent px-8 py-3 text-sm uppercase tracking-[0.2em] text-accent transition-colors hover:bg-accent hover:text-background disabled:cursor-not-allowed disabled:opacity-50 sm:w-auto"
      >
        {isPending ? "Booking..." : "Confirm Booking"}
      </button>
    </form>
  );
}
