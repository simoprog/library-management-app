import axios from "axios";

const API_BASE_URL = "https://localhost:7280/api";

export const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});
