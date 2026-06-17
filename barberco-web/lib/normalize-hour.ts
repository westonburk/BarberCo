import type { Hour } from "./types";

function extractTime(value: string): string {
  if (value.includes("T")) {
    const date = new Date(value);
    const hours = date.getHours().toString().padStart(2, "0");
    const minutes = date.getMinutes().toString().padStart(2, "0");
    return `${hours}:${minutes}`;
  }

  return value;
}

export function normalizeHour(hour: Hour): Hour {
  return {
    ...hour,
    startTime: extractTime(hour.startTime),
    endTime: extractTime(hour.endTime),
  };
}
