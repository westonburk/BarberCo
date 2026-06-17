import { apiFetch } from "./api-client";
import type { Appointment, AppointmentUpdateDto } from "./types";

export async function createAppointment(
  appointment: AppointmentUpdateDto,
): Promise<Appointment> {
  return apiFetch<Appointment>("appointment", {
    method: "POST",
    body: JSON.stringify(appointment),
  });
}
