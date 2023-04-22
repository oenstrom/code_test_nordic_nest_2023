import { useParams} from "react-router-dom"

export default function FetchSku() {
  const {sku} = useParams()
  const fetchPrice = async (sku) => {
    const response = await fetch(`Price/${sku}`)
    const data = await response.json()
    console.log(data)
  }
  console.log(sku)
  fetchPrice(sku)

  return (
    <div>
      SKU!
    </div>
  )
}