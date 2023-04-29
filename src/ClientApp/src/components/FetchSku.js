import { useEffect, useState } from 'react'
import { useParams} from "react-router-dom"

export default function FetchSku() {
  const {sku} = useParams()
  const [loading, setLoading] = useState(true)
  const [prices, setPrices] = useState([])
  const fetchPrice = async (sku) => {
    setLoading(true)
    const response = await fetch(`Price/${sku}`)
    const data = await response.json()
    setPrices(data)
    setLoading(false)
  }

  useEffect(() => {
    fetchPrice(sku)
  }, [sku])
  
  console.log(prices)
  
  const formatDate = (dateString) => {
    if (dateString === null) return ""
    const date = new Date(dateString)
    const year = date.getFullYear()
    const month = String(date.getMonth() + 1).padStart(2, '0')
    const day = String(date.getDate()).padStart(2, '0')
    const hour = String(date.getHours()).padStart(2, '0')
    const minute = String(date.getMinutes()).padStart(2, '0')
    return `${year}-${month}-${day} ${hour}:${minute}`
  }
    

  return (
    <div>
      <h1 id="tableLabel" className="text-3xl font-bold">SKU: {sku}</h1>

      {loading && <p><em>Loading...</em></p>}
      
      {!loading && prices.length > 0 ?
        <table className="w-full" aria-labelledby="tableLabel">
          <thead>
            <tr>
              <th>Marknad</th>
              <th>Pris</th>
              <th>Valuta</th>
              <th>Start och slut</th>
            </tr>
          </thead>
          <tbody>
          {prices.map(group =>
            group.map((p, i) =>
            <tr key={`${i}-${p.priceValueId}`} className="odd:bg-white even:bg-slate-100 text-center">
              <td className="p-2">{p.marketId}</td>
              <td className="p-2">{p.unitPrice}</td>
              <td className="p-2">{p.currencyCode}</td>
              <td className="p-2">
                <div className="flex">
                {formatDate(p.validFrom)} - {formatDate(p.validUntil)}
                </div>
              </td>
            </tr>
          ))}
          </tbody>
        </table>
      : <p>Ingen data hittades!</p>}
    </div>
  )
}