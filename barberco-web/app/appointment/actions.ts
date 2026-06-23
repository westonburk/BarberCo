"use server";

import { isRedirectError } from "next/dist/client/components/redirect-error";
import { redirect } from "next/navigation";
import { ApiError } from "@/lib/api-client";
import { createAppointment } from "@/lib/create-appointment";
import { isLocalDateTimeString } from "@/lib/format-local-datetime";
import { PHONE_PATTERN } from "@/lib/format-phone";
import type { AppointmentFormState } from "@/lib/appointment-form-state";

export async function submitAppointment(
  _prevState: AppointmentFormState,
  formData: FormData,
): Promise<AppointmentFormState> {
  const name = String(formData.get("name") ?? "").trim();
  const phone = String(formData.get("phone") ?? "").trim();
  const date = String(formData.get("date") ?? "");
  const time = String(formData.get("time") ?? "");
  const serviceIds = formData
    .getAll("serviceIds")
    .map((value) => Number(value))
    .filter((value) => !Number.isNaN(value));

  if (!name) {
    return { error: "Name is required." };
  }

  if (!PHONE_PATTERN.test(phone)) {
    return { error: "Phone must be in format (123) 456-7890." };
  }

  if (!date) {
    return { error: "Date is required." };
  }

  if (!time) {
    return { error: "Time is required." };
  }

  if (serviceIds.length === 0) {
    return { error: "Select at least one service." };
  }

  if (!isLocalDateTimeString(time)) {
    return { error: "Invalid appointment time." };
  }

  try {
    const appointment = await createAppointment({
      customerName: name,
      customerPhone: phone,
      dateTime: time,
      serviceIds,
    });

    redirect(
      `/appointment/success?at=${encodeURIComponent(appointment.dateTime)}`,
    );
  } catch (error) {
    if (isRedirectError(error)) {
      throw error;
    }

    if (error instanceof ApiError) {
      if (error.status === 422) {
        return { error: error.message };
      }

      if (error.status === 0) {
        return {
          error:
            "Booking is not configured yet. Add API_BASE_URL and API_KEY to .env.local.",
        };
      }

      return { error: error.message || "Failed to book appointment." };
    }

    throw error;
  }
}
