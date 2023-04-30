import { Fragment, useEffect, useState } from 'react'
import { useParams} from "react-router-dom"
import NavigateSku from "./NavigateSku"

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
    <div className="h-full w-full p-4 flex flex-col overflow-auto">
      <div className="flex w-full justify-center items-center mb-4">
        <NavigateSku preFilledSku={sku} />
      </div>
      {loading && <p className="text-center text-white text-lg">Loading...</p>}
      {!loading && prices.status &&  <p className="text-center text-white text-lg">No data found! 😢</p>}
      
      {!loading && prices.length > 0 &&
        <table className="w-full table-fixed border-separate border-spacing-x-0 border-spacing-y-px mt-4" aria-labelledby="tableLabel">
          <caption className="caption-top text-3xl font-bold text-slate-100 underline underline-offset-2 mb-2">SKU: {sku}</caption>
          <thead className="sticky -top-5 bg-slate-600 text-white">
            <tr className="rounded shadow">
              <th className="w-2/12 p-4 rounded-tl">Market</th>
              <th className="w-3/12 p-4">Price</th>
              <th className="w-2/12 p-4">Currency</th>
              <th className="p-4 rounded-tr">Start and End</th>
            </tr>
          </thead>
          <tbody>
          {prices.map((group, i) =>
            <Fragment key={`_${i}`}>
              {group.map((p, j) =>
                <tr key={`${j}-${p.priceValueId}`} className="bg-slate-600 text-slate-100 border-b border-b-slate-500 text-center">
                  <td className="p-2">{p.marketId}</td>
                  <td className="p-2">{p.unitPrice.toFixed(2)}</td>
                  <td className="p-2">{p.currencyCode}</td>
                  <td className="p-2">
                    {formatDate(p.validFrom)} - {formatDate(p.validUntil)}
                  </td>
                </tr>
              )}
              <tr className="h-6 bg-transparent last:h-1 last:bg-slate-600 last:-translate-y-px">
                <td className="rounded-bl"></td>
                <td></td>
                <td></td>
                <td className="rounded-br"></td>
              </tr>
            </Fragment>
          )}
          </tbody>
        </table>}
    </div>
  )
}