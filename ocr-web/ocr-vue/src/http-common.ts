import axios from "axios";
// import * as process from "process";

const port = process.env.NODE_ENV === "production" ? "5001" : "7256";

export default axios.create({
  baseURL: `https://localhost:${port}/`,
  headers: {
    "Content-type": "application/json",
  },
});
