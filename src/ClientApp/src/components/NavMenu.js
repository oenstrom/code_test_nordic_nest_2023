import { Link } from 'react-router-dom'

export default function NavMenu() {
  return (
    <header className="flex">
      <Link to="/">Optimized Price</Link>
      <nav className="flex">
        <ul className="navbar-nav flex-grow">
          <Link className="text-dark" to="/">Home</Link>
          <Link className="text-dark" to="/sku/27773-02">Fetch SKU</Link>
        </ul>
      </nav>
    </header>
  )
}
