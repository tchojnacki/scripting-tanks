const SERVER_HOST =
  process.env.NODE_ENV === "production" ? window.location.host : `${window.location.hostname}:3000`

export const WEBSOCKET_ROOT = `${
  window.location.protocol === "https:" ? "wss:" : "ws:"
}//${SERVER_HOST}/ws`

export const API_ROOT = `${window.location.protocol}//${SERVER_HOST}/api`
