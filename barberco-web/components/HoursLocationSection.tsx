import { OrnamentedHeading } from "@/components/OrnamentedHeading";
import { formatHour } from "@/lib/format-hour";
import { getHours } from "@/lib/get-hours";

const CONTACT = {
  email: "example@email.com",
  phone: "(123) 456-7890",
  address: "20 W 34th St., New York, NY 10001",
  mapsUrl: "https://maps.app.goo.gl/yKo3WYLA3aU5LCt2A",
  embedUrl:
    "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d6312.862677093851!2d-73.98823395938666!3d40.74844047150719!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x89c259a9b3117469%3A0xd134e199a405a163!2sEmpire%20State%20Building!5e1!3m2!1sen!2sus!4v1754778205889!5m2!1sen!2sus",
};

export async function HoursLocationSection() {
  const hours = await getHours();

  return (
    <section className="px-6 py-12 md:py-16">
      <div className="mx-auto grid max-w-6xl gap-16 md:grid-cols-2">
        <div>
          <OrnamentedHeading>Hours & Location</OrnamentedHeading>

          <ul className="mt-8 space-y-3">
            {hours.map((hour) => (
              <li
                key={hour.id}
                className="flex items-baseline justify-between gap-4 border-b border-foreground/10 pb-3 text-sm"
              >
                <span className="font-serif text-base tracking-wide">
                  {hour.dayOfWeek}
                </span>
                <span
                  className={
                    hour.isClosed ? "text-muted" : "tabular-nums text-foreground/90"
                  }
                >
                  {formatHour(hour)}
                </span>
              </li>
            ))}
          </ul>

          <div className="mt-10 space-y-4 text-sm">
            <p>
              <span className="text-muted">Phone · </span>
              <a
                href={`tel:${CONTACT.phone.replace(/\D/g, "")}`}
                className="transition-colors hover:text-accent"
              >
                {CONTACT.phone}
              </a>
            </p>
            <p>
              <span className="text-muted">Email · </span>
              <a
                href={`mailto:${CONTACT.email}`}
                className="transition-colors hover:text-accent"
              >
                {CONTACT.email}
              </a>
            </p>
            <p>
              <span className="text-muted">Address · </span>
              <a
                href={CONTACT.mapsUrl}
                target="_blank"
                rel="noopener noreferrer"
                className="transition-colors hover:text-accent"
              >
                {CONTACT.address}
              </a>
            </p>
          </div>
        </div>

        <div className="relative min-h-[320px] overflow-hidden border border-foreground/10 md:min-h-full">
          <iframe
            src={CONTACT.embedUrl}
            className="absolute inset-0 h-full w-full grayscale invert"
            loading="lazy"
            referrerPolicy="no-referrer-when-downgrade"
            title="Mike's Barber Shop location on Google Maps"
          />
        </div>
      </div>
    </section>
  );
}
