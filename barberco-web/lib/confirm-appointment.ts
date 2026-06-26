import { apiFetch } from "./api-client";
import type { ConfirmAppointmentDto, ConfirmedAppointment } from "./types";

/**
 * Submits the SMS confirmation code for an unconfirmed appointment. On success
 * the API marks the appointment confirmed and returns its details.
 */
export async function confirmAppointment(
  dto: ConfirmAppointmentDto,
): Promise<ConfirmedAppointment> {
  return apiFetch<ConfirmedAppointment>("appointment/confirm", {
    method: "POST",
    body: JSON.stringify(dto),
  });
}
