import Image from "next/image";
import Link from "next/link";
import { AppointmentForm } from "@/components/AppointmentForm";
import { OrnamentedHeading } from "@/components/OrnamentedHeading";
import { getHours } from "@/lib/get-hours";
import { getServices } from "@/lib/get-services";

export const dynamic = "force-dynamic";

export default async function AppointmentPage() {
  const [services, hours] = await Promise.all([getServices(), getHours()]);

  return (
    <main className="min-h-screen">
      <div className="grid min-h-screen md:grid-cols-2">
        <div className="relative min-h-64 md:min-h-full">
          <Image
            src="/images/appointment.jpg"
            alt="Classic barbershop stations with brick walls and pendant lighting"
            fill
            className="object-cover"
            priority
            sizes="(max-width: 768px) 100vw, 50vw"
          />
          <div className="image-vignette pointer-events-none absolute inset-0" />
          <div className="absolute inset-0 bg-gradient-to-t from-background via-background/20 to-transparent md:bg-gradient-to-r md:from-transparent md:via-background/30 md:to-background" />
        </div>

        <div className="flex flex-col justify-center px-6 py-16 md:px-12 md:py-24">
          <OrnamentedHeading as="h1" className="!mb-4">
            Book an Appointment
          </OrnamentedHeading>
          <p className="mb-8 max-w-md text-muted">
            Choose your services, pick a time, and we&apos;ll see you in the
            chair.
          </p>

          <AppointmentForm services={services} hours={hours} />

          <Link
            href="/"
            className="mt-8 text-sm text-muted transition-colors hover:text-accent"
          >
            &larr; Back to home
          </Link>
        </div>
      </div>
    </main>
  );
}
