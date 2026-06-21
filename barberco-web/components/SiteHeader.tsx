import { Logo } from "@/components/Logo";

export function SiteHeader() {
  return (
    <header className="fixed top-0 z-50 w-full px-6 py-5">
      <div className="mx-auto flex max-w-6xl justify-center md:justify-start">
        <Logo size={32} />
      </div>
    </header>
  );
}
