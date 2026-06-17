export default function Home() {
  return (
    <main>
      <div className="relative overflow-hidden text-center">
        <img
          src="/images/hero.png"
          alt="Barber shop interior"
          className="block w-full opacity-40"
        />
        <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2">
          <h1>Ready for a Fresh Cut?</h1>
          <p>Stop by Mike&apos;s Today.</p>
        </div>
      </div>
    </main>
  );
}