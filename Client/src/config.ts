const SERVER_HOST =
  process.env.NODE_ENV === "production" ? window.location.host : `${window.location.hostname}:3000`

const WEBSOCKET_PROTOCOL = window.location.protocol === "https:" ? "wss:" : "ws:"

export const WEBSOCKET_ROOT = `${WEBSOCKET_PROTOCOL}//${SERVER_HOST}/ws`
