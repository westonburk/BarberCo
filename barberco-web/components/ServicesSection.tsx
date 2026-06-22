import Link from "next/link";
import { OrnamentedHeading } from "@/components/OrnamentedHeading";
import { formatPrice } from "@/lib/format-price";
import { getServices } from "@/lib/get-services";

export async function ServicesSection() {
  const services = await getServices();

  return (
    <section className="px-6 pt-6 pb-12 md:pt-8 md:pb-16">
      <div className="mx-auto max-w-3xl">
        <header className="mb-10">
          <OrnamentedHeading align="center">Services</OrnamentedHeading>
        </header>

        <ul>
          {services.map((service) => (
            <li key={service.id} className="group py-6">
              <div className="flex items-baseline gap-4">
                <span className="font-serif text-xl tracking-wide transition-colors group-hover:text-accent md:text-2xl">
                  {service.name}
                </span>
                <span
                  className="mb-1.5 min-w-8 flex-1 border-b border-dotted border-foreground/20"
                  aria-hidden="true"
                />
                <span className="shrink-0 font-sans text-lg tabular-nums text-accent">
                  {formatPrice(service.price)}
                </span>
              </div>
            </li>
          ))}
        </ul>

        <div className="mt-12 text-center">
          <Link
            href="/appointment"
            className="inline-block border border-accent px-8 py-3 text-sm uppercase tracking-[0.2em] text-accent transition-colors hover:bg-accent hover:text-background"
          >
            Book an Appointment
          </Link>
        </div>
      </div>
    </section>
  );
}
