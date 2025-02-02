// eslint-disable-next-line @typescript-eslint/no-explicit-any
export default function Header({ title }: any) {
  return (
    <header className="bg-blue-500 text-white p-4">
      <h1 className="text-2xl font-bold">{title}</h1>
    </header>
  );
}
