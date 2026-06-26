import { apiFetch } from "./api-client";
import type { AppointmentUpdateDto } from "./types";

/**
 * Creates an unconfirmed appointment via the public web endpoint. The API
 * stores the appointment, texts a confirmation code to the customer, and
 * returns the id of the unconfirmed record.
 */
export async function createAppointment(
  appointment: AppointmentUpdateDto,
): Promise<number> {
  return apiFetch<number>("appointment/create/web", {
    method: "POST",
    body: JSON.stringify(appointment),
  });
}
