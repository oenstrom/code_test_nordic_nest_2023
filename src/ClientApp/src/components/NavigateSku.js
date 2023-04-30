import { useState } from 'react'
import { useNavigate } from 'react-router-dom'

export default function NavigateSku({ preFilledSku }) {
  const [sku, setSku] = useState(preFilledSku || '')
  const navigate = useNavigate()
  
  const onKeyEnter = e =>
    e.key === 'Enter' && navigate(`/sku/${sku}`)

  return (
    <input
      type="text"
      value={sku}
      onChange={e => setSku(e.target.value)}
      onKeyDown={onKeyEnter}
      placeholder="Enter SKU..."
      autoFocus={true}
      className={`
        text-center text-white outline-none ring-none border-b border-b-slate-200 text-4xl bg-transparent w-1/3
        transition-all py-1 px-2
        focus:ring-none focus:w-2/4 focus:border-b-white
        placeholder:text-slate-200 focus:placeholder:text-white
      `}
    />
  )
}
