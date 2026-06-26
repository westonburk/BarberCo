"use server";

import { ApiError } from "@/lib/api-client";
import { confirmAppointment as requestConfirmation } from "@/lib/confirm-appointment";
import { createAppointment } from "@/lib/create-appointment";
import { isLocalDateTimeString } from "@/lib/format-local-datetime";
import { PHONE_PATTERN } from "@/lib/format-phone";
import type {
  AppointmentFormState,
  ConfirmFormState,
} from "@/lib/appointment-form-state";

const NOT_CONFIGURED_MESSAGE =
  "Booking is not configured yet. Add API_BASE_URL and API_KEY to .env.local.";

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
    const appointmentId = await createAppointment({
      customerName: name,
      customerPhone: phone,
      dateTime: time,
      serviceIds,
    });

    return { appointmentId };
  } catch (error) {
    if (error instanceof ApiError) {
      if (error.status === 422) {
        return { error: error.message };
      }

      if (error.status === 0) {
        return { error: NOT_CONFIGURED_MESSAGE };
      }

      return { error: error.message || "Failed to book appointment." };
    }

    throw error;
  }
}

export async function confirmAppointment(
  _prevState: ConfirmFormState,
  formData: FormData,
): Promise<ConfirmFormState> {
  const appointmentId = Number(formData.get("appointmentId"));
  const confirmationCode = String(formData.get("code") ?? "").trim();

  if (!appointmentId || Number.isNaN(appointmentId)) {
    return { error: "Something went wrong. Please start your booking again." };
  }

  if (!/^\d{6}$/.test(confirmationCode)) {
    return { error: "Enter the 6-digit code from your text message." };
  }

  try {
    const confirmed = await requestConfirmation({
      appointmentId,
      confirmationCode,
    });

    return { confirmed };
  } catch (error) {
    if (error instanceof ApiError) {
      if (error.status === 400) {
        return {
          error:
            "That code is invalid or expired. Please start your booking again.",
        };
      }

      if (error.status === 0) {
        return { error: NOT_CONFIGURED_MESSAGE };
      }

      return { error: error.message || "Could not confirm your appointment." };
    }

    throw error;
  }
}
