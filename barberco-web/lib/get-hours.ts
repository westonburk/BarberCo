import { apiFetch } from "./api-client";
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

function sortHours(hours: Hour[]): Hour[] {
  return [...hours].sort(
    (a, b) =>
      (DAY_ORDER[a.dayOfWeek.toLowerCase()] ?? 7) -
      (DAY_ORDER[b.dayOfWeek.toLowerCase()] ?? 7),
  );
}

export async function getHours(): Promise<Hour[]> {
  const hours = await apiFetch<Hour[]>("hour");
  return sortHours(hours.map(normalizeHour));
}
