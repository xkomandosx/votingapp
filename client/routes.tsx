import App from "./views/App";
import Dashboard from "./views/Dashboard";
import CandidatePage from "./views/CandidatePage";
import VoterPage from "./views/VoterPage";

// Import all the routeable views into the global window variable.
Object.assign(window, {
  Dashboard,
  CandidatePage,
  VoterPage,
});

export default App;
