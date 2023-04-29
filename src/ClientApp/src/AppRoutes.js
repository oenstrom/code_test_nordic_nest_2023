import Home from "./components/Home"
import FetchSku from "./components/FetchSku"

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/sku/:sku',
    element: <FetchSku />
  }
]

export default AppRoutes
