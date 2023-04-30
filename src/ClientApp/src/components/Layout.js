import NavMenu from './NavMenu'

export default function Layout({ children }) {
  return (
    <div className="h-screen flex justify-center items-center bg-gradient-to-b from-slate-500 via-slate-400 to-slate-500">
        {children}
    </div>
  )
}
