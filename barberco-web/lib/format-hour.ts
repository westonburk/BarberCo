import type { Hour } from "./types";

function formatTime(time: string): string {
  const [hours, minutes] = time.split(":").map(Number);
  const date = new Date();
  date.setHours(hours, minutes, 0, 0);

  return new Intl.DateTimeFormat("en-US", {
    hour: "numeric",
    minute: minutes === 0 ? undefined : "2-digit",
  }).format(date);
}

export function formatHour(hour: Hour): string {
  if (hour.isClosed) {
    return "Closed";
  }

  return `${formatTime(hour.startTime)} – ${formatTime(hour.endTime)}`;
}
