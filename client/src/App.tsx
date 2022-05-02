import { useEffect, useRef } from 'react';
import logo from './logo.svg';
import './App.css';

const client_id = Date.now();

const SERVER_HOST = process.env.NODE_ENV === 'production'
  ? window.location.host
  : 'localhost:3000';

const WEBSOCKET_ROOT = `${window.location.protocol === "https:" ? "wss:" : "ws:"}//${SERVER_HOST}/ws/${client_id}`;
const API_ROOT = `${window.location.protocol}//${SERVER_HOST}/api`;

function App() {
  const wsRef = useRef<WebSocket>();

  function sendMessage(msg: string) {
    wsRef.current?.send(msg);
  }

  useEffect(() => {
    const ws = new WebSocket(WEBSOCKET_ROOT);

    ws.onmessage = function(event) {
      console.log(event);
    };
    
    wsRef.current = ws;

    return () => {
      ws.close();
    }
  }, []);

  useEffect(() => {
    fetch(`${API_ROOT}/hello`).then(resp => resp.json()).then(json => console.log(json));
  }, []);

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.tsx</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
    </div>
  );
}

export default App;
