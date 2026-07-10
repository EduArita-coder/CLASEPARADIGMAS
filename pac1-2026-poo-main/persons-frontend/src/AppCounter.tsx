import { Minus, Plus, RotateCcw } from "lucide-react"
import { useState } from "react"
import './AppCounter.css';
function AppCounter() {
      //[0, 1]
  const [count, setCount] = useState(101);

  const increment = function () {
    setCount(prev => prev + 1);
  }

  const decrement = () => setCount(prev => prev - 1);
  const reset = () => setCount(0);
    
  return (
    <div className="container">
      <div className="counter-card">
        <h1 className="title">Contador</h1>

        <div className="count">
          {count}
        </div>

        <div className="buttons">
          <button onClick={decrement} className="button button-decrease">
            <Minus size={24} />
          </button>
          <button onClick={reset} className="button button-reset">
            <RotateCcw size={24} />
          </button>
          <button onClick={increment} className="button button-increase">
            <Plus size={24} />
          </button>
        </div>

      </div>
    </div>
  )
}

export default AppCounter
