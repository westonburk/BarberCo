import type { Hour, TimeSlot } from "./types";
import { formatLocalDateTime } from "./format-local-datetime";

const DAY_NAMES = [
  "Sunday",
  "Monday",
  "Tuesday",
  "Wednesday",
  "Thursday",
  "Friday",
  "Saturday",
];

function startOfDay(date: Date): Date {
  const copy = new Date(date);
  copy.setHours(0, 0, 0, 0);
  return copy;
}

function isSameDay(a: Date, b: Date): boolean {
  return (
    a.getFullYear() === b.getFullYear() &&
    a.getMonth() === b.getMonth() &&
    a.getDate() === b.getDate()
  );
}

function parseTimeOnDate(date: Date, time: string): Date {
  const [hours, minutes] = time.split(":").map(Number);
  const copy = new Date(date);
  copy.setHours(hours, minutes, 0, 0);
  return copy;
}

function formatDisplayTime(date: Date): string {
  return new Intl.DateTimeFormat("en-US", {
    hour: "numeric",
    hour12: true,
  }).format(date);
}

function getHoursBetweenDates(start: Date, end: Date): TimeSlot[] {
  const results: TimeSlot[] = [];
  const cursor = new Date(start);

  while (cursor <= end) {
    results.push({
      value: formatLocalDateTime(cursor),
      display: formatDisplayTime(cursor),
    });
    cursor.setHours(cursor.getHours() + 1);
  }

  return results;
}

export function getValidTimesForDay(dateInput: Date, hours: Hour[]): TimeSlot[] {
  const now = new Date();
  const date = new Date(dateInput);

  if (startOfDay(date) < startOfDay(now)) {
    return [];
  }

  const dayName = DAY_NAMES[date.getDay()];
  const hour = hours.find((entry) => entry.dayOfWeek === dayName);

  if (!hour || hour.isClosed) {
    return [];
  }

  let shiftStart = parseTimeOnDate(date, hour.startTime);
  const shiftEnd = parseTimeOnDate(date, hour.endTime);

  if (isSameDay(date, now) && now > shiftStart) {
    shiftStart = new Date(date);
    shiftStart.setHours(now.getHours() + 1, 0, 0, 0);
  }

  return getHoursBetweenDates(shiftStart, shiftEnd);
}
