import Link from "next/link";

type SuccessPageProps = {
  searchParams: Promise<{ at?: string }>;
};

function formatAppointmentDateTime(value: string): {
  date: string;
  time: string;
} | null {
  const dateTime = new Date(value);
  if (Number.isNaN(dateTime.getTime())) {
    return null;
  }

  return {
    date: new Intl.DateTimeFormat("en-US", {
      weekday: "long",
      month: "long",
      day: "numeric",
      year: "numeric",
    }).format(dateTime),
    time: new Intl.DateTimeFormat("en-US", {
      hour: "numeric",
      minute: "2-digit",
    }).format(dateTime),
  };
}

export default async function AppointmentSuccessPage({
  searchParams,
}: SuccessPageProps) {
  const { at } = await searchParams;
  const formatted = at ? formatAppointmentDateTime(at) : null;

  return (
    <main className="flex min-h-screen items-center justify-center px-6 py-24">
      <div className="max-w-lg text-center">
        <div className="mx-auto mb-8 flex h-20 w-20 items-center justify-center rounded-full border border-accent text-accent">
          <svg
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            strokeWidth="1.5"
            className="h-10 w-10"
            aria-hidden="true"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              d="M5 13l4 4L19 7"
            />
          </svg>
        </div>

        <p className="mb-4 text-sm uppercase tracking-[0.3em] text-accent">
          Confirmed
        </p>
        <h1 className="font-serif text-4xl tracking-wide md:text-5xl">
          You&apos;re Booked
        </h1>

        {formatted ? (
          <p className="mt-6 text-lg text-muted">
            {formatted.date}
            <br />
            {formatted.time}
          </p>
        ) : (
          <p className="mt-6 text-muted">
            Your appointment has been confirmed.
          </p>
        )}

        <Link
          href="/"
          className="mt-10 inline-block border border-accent px-8 py-3 text-sm uppercase tracking-[0.2em] text-accent transition-colors hover:bg-accent hover:text-background"
        >
          Return Home
        </Link>
      </div>
    </main>
  );
}
