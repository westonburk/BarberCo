import { apiFetch, isApiConfigured } from "./api-client";
import { normalizeHour } from "./normalize-hour";
import type { Hour } from "./types";

const DAY_ORDER: Record<string, number> = {
  monday: 0,
  tuesday: 1,
  wednesday: 2,
  thursday: 3,
  friday: 4,
  saturday: 5,
  sunday: 6,
};

const PLACEHOLDER_HOURS: Hour[] = [
  { id: 1, dayOfWeek: "Monday", startTime: "09:00", endTime: "19:00", isClosed: false },
  { id: 2, dayOfWeek: "Tuesday", startTime: "09:00", endTime: "19:00", isClosed: false },
  { id: 3, dayOfWeek: "Wednesday", startTime: "09:00", endTime: "19:00", isClosed: false },
  { id: 4, dayOfWeek: "Thursday", startTime: "09:00", endTime: "19:00", isClosed: false },
  { id: 5, dayOfWeek: "Friday", startTime: "09:00", endTime: "19:00", isClosed: false },
  { id: 6, dayOfWeek: "Saturday", startTime: "08:00", endTime: "17:00", isClosed: false },
  { id: 7, dayOfWeek: "Sunday", startTime: "00:00", endTime: "00:00", isClosed: true },
];

function sortHours(hours: Hour[]): Hour[] {
  return [...hours].sort(
    (a, b) =>
      (DAY_ORDER[a.dayOfWeek.toLowerCase()] ?? 7) -
      (DAY_ORDER[b.dayOfWeek.toLowerCase()] ?? 7),
  );
}

export async function getHours(): Promise<Hour[]> {
  if (isApiConfigured()) {
    try {
      const hours = await apiFetch<Hour[]>("hour");
      return sortHours(hours.map(normalizeHour));
    } catch (error) {
      console.error("Failed to fetch hours from API, using placeholders:", error);
    }
  }

  return sortHours(PLACEHOLDER_HOURS);
}
