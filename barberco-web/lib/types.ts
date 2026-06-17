export interface Service {
  id: number;
  name: string;
  price: number;
}

export interface Hour {
  id: number;
  dayOfWeek: string;
  startTime: string;
  endTime: string;
  isClosed: boolean;
}

export interface TimeSlot {
  value: string;
  display: string;
}

export interface AppointmentUpdateDto {
  customerName: string;
  customerPhone: string;
  dateTime: string;
  serviceIds: number[];
}

export interface Appointment {
  id: number;
  customerName: string;
  customerPhone: string;
  dateTime: string;
}
