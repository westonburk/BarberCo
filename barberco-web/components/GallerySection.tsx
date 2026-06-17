"use client";

import Image from "next/image";
import { useCallback, useEffect, useRef, useState } from "react";

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

const AUTO_PLAY_MS = 5000;
const PAUSE_AFTER_INTERACTION_MS = 8000;

export function GallerySection() {
  const [activeIndex, setActiveIndex] = useState(0);
  const pausedUntilRef = useRef(0);
  const isHoveredRef = useRef(false);
  const touchStartXRef = useRef<number | null>(null);
  const thumbnailStripRef = useRef<HTMLDivElement | null>(null);
  const thumbnailRefs = useRef<(HTMLButtonElement | null)[]>([]);

  const pauseAutoplay = useCallback((ms = PAUSE_AFTER_INTERACTION_MS) => {
    pausedUntilRef.current = Date.now() + ms;
  }, []);

  const goTo = useCallback(
    (index: number) => {
      const wrapped =
        ((index % GALLERY_IMAGES.length) + GALLERY_IMAGES.length) %
        GALLERY_IMAGES.length;
      setActiveIndex(wrapped);
      pauseAutoplay();
    },
    [pauseAutoplay],
  );

  const goNext = useCallback(() => {
    goTo(activeIndex + 1);
  }, [activeIndex, goTo]);

  const goPrev = useCallback(() => {
    goTo(activeIndex - 1);
  }, [activeIndex, goTo]);

  const scrollThumbnailIntoView = useCallback((index: number) => {
    const strip = thumbnailStripRef.current;
    const thumb = thumbnailRefs.current[index];
    if (!strip || !thumb) return;

    const targetLeft =
      thumb.offsetLeft - strip.clientWidth / 2 + thumb.clientWidth / 2;

    strip.scrollTo({ left: targetLeft, behavior: "smooth" });
  }, []);

  useEffect(() => {
    const interval = window.setInterval(() => {
      if (isHoveredRef.current) return;
      if (Date.now() < pausedUntilRef.current) return;
      setActiveIndex((current) => (current + 1) % GALLERY_IMAGES.length);
    }, AUTO_PLAY_MS);

    return () => window.clearInterval(interval);
  }, []);

  function handleTouchStart(event: React.TouchEvent) {
    touchStartXRef.current = event.touches[0]?.clientX ?? null;
  }

  function handleTouchEnd(event: React.TouchEvent) {
    const startX = touchStartXRef.current;
    const endX = event.changedTouches[0]?.clientX;
    touchStartXRef.current = null;

    if (startX == null || endX == null) return;

    const delta = endX - startX;
    if (Math.abs(delta) < 50) return;

    if (delta < 0) {
      goNext();
    } else {
      goPrev();
    }
  }

  return (
    <section className="px-6 py-24 md:py-32">
      <div className="mx-auto max-w-6xl">
        <header className="mb-10 text-center md:mb-16">
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

        <div
          className="relative w-full"
          onTouchStart={handleTouchStart}
          onTouchEnd={handleTouchEnd}
          aria-roledescription="carousel"
          aria-label="Barbershop photo gallery"
        >
          <div
            className="relative aspect-[4/5] w-full overflow-hidden md:aspect-[16/10]"
            onMouseEnter={() => {
              isHoveredRef.current = true;
            }}
            onMouseLeave={() => {
              isHoveredRef.current = false;
            }}
          >
            {GALLERY_IMAGES.map((image, index) => {
              const isActive = index === activeIndex;

              return (
                <div
                  key={image.src}
                  className={`absolute inset-0 transition-all duration-[1400ms] ease-in-out ${
                    isActive
                      ? "scale-100 opacity-100"
                      : "pointer-events-none scale-[1.03] opacity-0"
                  }`}
                  aria-hidden={!isActive}
                >
                  <Image
                    src={image.src}
                    alt={image.alt}
                    fill
                    className="object-cover"
                    priority={index === 0}
                    sizes="(max-width: 1152px) 100vw, 1152px"
                  />
                </div>
              );
            })}
            <div className="image-vignette pointer-events-none absolute inset-0" />
          </div>

          <div
            ref={thumbnailStripRef}
            className="scrollbar-none mt-8 flex justify-start gap-2 overflow-x-auto pb-2 md:justify-center md:gap-3"
          >
            {GALLERY_IMAGES.map((image, index) => (
              <button
                key={image.src}
                ref={(element) => {
                  thumbnailRefs.current[index] = element;
                }}
                type="button"
                onClick={() => {
                  goTo(index);
                  scrollThumbnailIntoView(index);
                }}
                className={`relative h-14 w-11 shrink-0 overflow-hidden transition-all duration-300 md:h-16 md:w-14 ${
                  index === activeIndex
                    ? "opacity-100 ring-1 ring-accent"
                    : "opacity-40 hover:opacity-70"
                }`}
                aria-label={`View photo ${index + 1}: ${image.alt}`}
                aria-current={index === activeIndex}
              >
                <Image
                  src={image.src}
                  alt=""
                  fill
                  className="object-cover"
                  sizes="56px"
                />
              </button>
            ))}
          </div>
        </div>
      </div>
    </section>
  );
}
