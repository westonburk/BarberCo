import Image from "next/image";

const GALLERY_IMAGES = [
  { src: "/images/gallery/gallery-1.jpg", alt: "Barber working on a client's haircut" },
  { src: "/images/gallery/gallery-2.jpg", alt: "Client receiving a straight-razor shave" },
  { src: "/images/gallery/gallery-3.jpg", alt: "Close-up of barber tools on a wooden counter" },
  { src: "/images/gallery/gallery-4.jpg", alt: "Barber styling a client's hair" },
  { src: "/images/gallery/gallery-5.jpg", alt: "Barbershop interior with patterned tile floor" },
  { src: "/images/gallery/gallery-6.jpg", alt: "Barber giving a client a precise fade" },
  { src: "/images/gallery/gallery-7.jpg", alt: "Wide view of a classic barbershop interior" },
  { src: "/images/gallery/gallery-8.jpg", alt: "Ornate classic barbershop with vintage chairs" },
  { src: "/images/gallery/gallery-9.jpg", alt: "Barber giving a client a precise fade" },
  { src: "/images/gallery/gallery-10.jpg", alt: "Traditional barbershop storefront and interior details" },
];

export function GallerySection() {
  return (
    <section className="px-6 py-24 md:py-32">
      <div className="mx-auto max-w-6xl">
        <header className="mb-16 text-center">
          <p className="mb-4 text-sm uppercase tracking-[0.3em] text-accent">
            Gallery
          </p>
          <h2 className="font-serif text-4xl tracking-wide md:text-5xl">
            Inside the Shop
          </h2>
          <p className="mx-auto mt-4 max-w-md text-muted">
            A glimpse of the atmosphere, the craft, and the details that define
            Mike&apos;s.
          </p>
        </header>

        <div className="grid grid-cols-2 gap-3 md:grid-cols-5 md:gap-4">
          {GALLERY_IMAGES.map((image) => (
            <div
              key={image.src}
              className="relative aspect-[4/5] overflow-hidden"
            >
              <Image
                src={image.src}
                alt={image.alt}
                fill
                className="object-cover transition-transform duration-700 hover:scale-105"
              />
              <div className="image-vignette pointer-events-none absolute inset-0 opacity-60" />
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}
