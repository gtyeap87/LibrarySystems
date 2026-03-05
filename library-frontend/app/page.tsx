export default function Home() {
  return (
    <>
      <h1>Welcome to the Home Page</h1>
      <p>
        Testing the development environment. The backend API URL is:
        {process.env.NEXT_PUBLIC_API_URL}
      </p>

      <p>This going to be dashboard</p>
    </>
  );
}
