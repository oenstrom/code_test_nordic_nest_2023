import Home from "./components/Home"
import Counter from "./components/Counter"
import FetchData from "./components/FetchData"
import FetchSku from "./components/FetchSku"

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  },
  {
    path: '/sku/:sku',
    element: <FetchSku />
  }
]

export default AppRoutes
