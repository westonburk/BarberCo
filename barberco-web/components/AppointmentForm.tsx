"use client";

import { useActionState, useEffect, useMemo, useState } from "react";
import {
  initialAppointmentFormState,
  initialConfirmFormState,
  type AppointmentFormState,
  type ConfirmFormState,
} from "@/lib/appointment-form-state";
import { getValidTimesForDay } from "@/lib/appointment-times";
import { formatPrice } from "@/lib/format-price";
import { formatPhoneNumber } from "@/lib/format-phone";
import type { ConfirmedAppointment, Hour, Service } from "@/lib/types";
import { confirmAppointment, submitAppointment } from "@/app/appointment/actions";

function formatDateInput(date: Date): string {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const day = String(date.getDate()).padStart(2, "0");
  return `${year}-${month}-${day}`;
}

function formatAppointmentDateTime(value: string): { date: string; time: string } {
  const dateTime = new Date(value);
  if (Number.isNaN(dateTime.getTime())) {
    return { date: value, time: "" };
  }

  return {
    date: new Intl.DateTimeFormat("en-US", {
      weekday: "long",
      month: "long",
      day: "numeric",
      year: "numeric",
    }).format(dateTime),
    time: new Intl.DateTimeFormat("en-US", {
      hour: "numeric",
      minute: "2-digit",
    }).format(dateTime),
  };
}

const inputClassName =
  "w-full border border-foreground/15 bg-background px-4 py-3 text-foreground outline-none transition-colors focus:border-accent";

type AppointmentFormProps = {
  services: Service[];
  hours: Hour[];
};

export function AppointmentForm({ services, hours }: AppointmentFormProps) {
  const [bookingState, bookingAction, isBooking] = useActionState<
    AppointmentFormState,
    FormData
  >(submitAppointment, initialAppointmentFormState);

  const [confirmState, confirmAction, isConfirming] = useActionState<
    ConfirmFormState,
    FormData
  >(confirmAppointment, initialConfirmFormState);

  const [phone, setPhone] = useState("");
  const [selectedIds, setSelectedIds] = useState<number[]>([]);
  const [date, setDate] = useState(() => formatDateInput(new Date()));
  const [time, setTime] = useState("");
  const [showConfirm, setShowConfirm] = useState(false);

  const times = useMemo(() => {
    const selectedDate = new Date(`${date}T12:00:00`);
    if (Number.isNaN(selectedDate.getTime())) {
      return [];
    }
    return getValidTimesForDay(selectedDate, hours);
  }, [date, hours]);

  useEffect(() => {
    setTime("");
  }, [date]);

  useEffect(() => {
    if (bookingState.appointmentId != null) {
      setShowConfirm(true);
    }
  }, [bookingState.appointmentId]);

  function toggleService(id: number) {
    setSelectedIds((current) =>
      current.includes(id)
        ? current.filter((serviceId) => serviceId !== id)
        : [...current, id],
    );
  }

  if (confirmState.confirmed) {
    return (
      <ConfirmedAppointmentView
        confirmed={confirmState.confirmed}
        services={services}
      />
    );
  }

  return (
    <>
      <form action={bookingAction} className="mt-8 space-y-6">
        {bookingState.error && (
          <div
            className="border border-red-400/40 bg-red-950/20 px-4 py-3 text-sm text-red-300"
            role="alert"
          >
            {bookingState.error}
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
              disabled={times.length === 0}
              onChange={(event) => setTime(event.target.value)}
              className={inputClassName}
            >
              <option value="">
                {times.length === 0 ? "No availability" : "Select a time"}
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
          disabled={isBooking || selectedIds.length === 0}
          className="w-full border border-accent px-8 py-3 text-sm uppercase tracking-[0.2em] text-accent transition-colors hover:bg-accent hover:text-background disabled:cursor-not-allowed disabled:opacity-50 sm:w-auto"
        >
          {isBooking ? "Booking..." : "Confirm Booking"}
        </button>
      </form>

      {showConfirm && bookingState.appointmentId != null && (
        <ConfirmationCodeModal
          appointmentId={bookingState.appointmentId}
          phone={phone}
          confirmAction={confirmAction}
          isConfirming={isConfirming}
          error={confirmState.error}
          onBack={() => setShowConfirm(false)}
        />
      )}
    </>
  );
}

type ConfirmationCodeModalProps = {
  appointmentId: number;
  phone: string;
  confirmAction: (formData: FormData) => void;
  isConfirming: boolean;
  error?: string;
  onBack: () => void;
};

function ConfirmationCodeModal({
  appointmentId,
  phone,
  confirmAction,
  isConfirming,
  error,
  onBack,
}: ConfirmationCodeModalProps) {
  const [code, setCode] = useState("");

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-background/80 px-6 backdrop-blur-sm"
      role="dialog"
      aria-modal="true"
      aria-labelledby="confirm-heading"
    >
      <div className="w-full max-w-md border border-foreground/15 bg-background p-8 shadow-2xl">
        <p className="mb-2 text-sm uppercase tracking-[0.3em] text-accent">
          Verify
        </p>
        <h2
          id="confirm-heading"
          className="font-serif text-3xl tracking-wide"
        >
          Enter Your Code
        </h2>
        <p className="mt-4 text-sm text-muted">
          We sent a 6-digit confirmation code by text
          {phone ? (
            <>
              {" "}
              to <span className="text-foreground">{phone}</span>
            </>
          ) : null}
          . Enter it below to lock in your appointment.
        </p>

        <form action={confirmAction} className="mt-6 space-y-5">
          {error && (
            <div
              className="border border-red-400/40 bg-red-950/20 px-4 py-3 text-sm text-red-300"
              role="alert"
            >
              {error}
            </div>
          )}

          <input type="hidden" name="appointmentId" value={appointmentId} />

          <div>
            <label htmlFor="code" className="mb-2 block text-sm text-muted">
              Confirmation code
            </label>
            <input
              id="code"
              name="code"
              type="text"
              inputMode="numeric"
              autoComplete="one-time-code"
              required
              autoFocus
              value={code}
              onChange={(event) =>
                setCode(event.target.value.replace(/\D/g, "").slice(0, 6))
              }
              placeholder="123456"
              maxLength={6}
              className={`${inputClassName} text-center text-2xl tracking-[0.6em]`}
            />
          </div>

          <div className="flex flex-col gap-3 sm:flex-row-reverse">
            <button
              type="submit"
              disabled={isConfirming || code.length !== 6}
              className="w-full border border-accent px-8 py-3 text-sm uppercase tracking-[0.2em] text-accent transition-colors hover:bg-accent hover:text-background disabled:cursor-not-allowed disabled:opacity-50 sm:w-auto"
            >
              {isConfirming ? "Confirming..." : "Confirm"}
            </button>
            <button
              type="button"
              onClick={onBack}
              disabled={isConfirming}
              className="w-full border border-foreground/15 px-8 py-3 text-sm uppercase tracking-[0.2em] text-muted transition-colors hover:border-foreground/30 hover:text-foreground disabled:cursor-not-allowed disabled:opacity-50 sm:w-auto"
            >
              Back
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

type ConfirmedAppointmentViewProps = {
  confirmed: ConfirmedAppointment;
  services: Service[];
};

function ConfirmedAppointmentView({
  confirmed,
  services,
}: ConfirmedAppointmentViewProps) {
  const { date, time } = formatAppointmentDateTime(confirmed.dateTime);
  const bookedServices = services.filter((service) =>
    confirmed.serviceIds.includes(service.id),
  );
  const total = bookedServices.reduce(
    (sum, service) => sum + service.price,
    0,
  );

  return (
    <div className="mt-8">
      <div className="border border-accent/40 bg-accent/5 p-8">
        <div className="mb-6 flex items-center gap-4">
          <span className="flex h-12 w-12 shrink-0 items-center justify-center rounded-full border border-accent text-accent">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="1.5"
              className="h-6 w-6"
              aria-hidden="true"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M5 13l4 4L19 7"
              />
            </svg>
          </span>
          <div>
            <p className="text-sm uppercase tracking-[0.3em] text-accent">
              Confirmed
            </p>
            <h2 className="font-serif text-3xl tracking-wide">
              You&apos;re Booked
            </h2>
          </div>
        </div>

        <dl className="space-y-4 text-sm">
          <div className="flex justify-between gap-4 border-t border-foreground/10 pt-4">
            <dt className="text-muted">Name</dt>
            <dd className="text-right text-foreground">
              {confirmed.customerName}
            </dd>
          </div>
          <div className="flex justify-between gap-4 border-t border-foreground/10 pt-4">
            <dt className="text-muted">Phone</dt>
            <dd className="text-right text-foreground">
              {confirmed.customerPhone}
            </dd>
          </div>
          <div className="flex justify-between gap-4 border-t border-foreground/10 pt-4">
            <dt className="text-muted">When</dt>
            <dd className="text-right text-foreground">
              {date}
              {time ? (
                <>
                  <br />
                  {time}
                </>
              ) : null}
            </dd>
          </div>
          <div className="border-t border-foreground/10 pt-4">
            <dt className="mb-3 text-muted">Services</dt>
            <dd>
              <ul className="space-y-2">
                {bookedServices.map((service) => (
                  <li
                    key={service.id}
                    className="flex justify-between gap-4 text-foreground"
                  >
                    <span>{service.name}</span>
                    <span className="tabular-nums text-accent">
                      {formatPrice(service.price)}
                    </span>
                  </li>
                ))}
              </ul>
              {bookedServices.length > 0 && (
                <div className="mt-3 flex justify-between gap-4 border-t border-foreground/10 pt-3 text-foreground">
                  <span className="uppercase tracking-[0.2em] text-muted">
                    Total
                  </span>
                  <span className="tabular-nums text-accent">
                    {formatPrice(total)}
                  </span>
                </div>
              )}
            </dd>
          </div>
        </dl>
      </div>
    </div>
  );
}
