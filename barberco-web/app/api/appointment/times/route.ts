import { getHours } from "@/lib/get-hours";
import { getValidTimesForDay } from "@/lib/appointment-times";

export async function GET(request: Request) {
  const dateParam = new URL(request.url).searchParams.get("date");

  if (!dateParam) {
    return Response.json({ error: "date is required" }, { status: 400 });
  }

  const date = new Date(`${dateParam}T12:00:00`);
  if (Number.isNaN(date.getTime())) {
    return Response.json({ error: "invalid date" }, { status: 400 });
  }

  const hours = await getHours();
  const times = getValidTimesForDay(date, hours);

  return Response.json({ times });
}
