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
          {prices.map(p => 
            <tr key={p.priceValueId} className="odd:bg-white even:bg-slate-100 text-center">
              <td className="p-2">{p.marketId}</td>
              <td className="p-2">{p.unitPrice}</td>
              <td className="p-2">{p.currencyCode}</td>
              <td className="p-2">{p.validFrom} - {p.validUntil}</td>
            </tr>
          )}
          </tbody>
        </table>
      : <p>Ingen data hittades!</p>}
    </div>
  )
}