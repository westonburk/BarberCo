import type { ConfirmedAppointment } from "./types";

export type AppointmentFormState = {
  error?: string;
  appointmentId?: number;
};

export const initialAppointmentFormState: AppointmentFormState = {};

export type ConfirmFormState = {
  error?: string;
  confirmed?: ConfirmedAppointment;
};

export const initialConfirmFormState: ConfirmFormState = {};
