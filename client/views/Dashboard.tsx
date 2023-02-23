import React, { ReactElement, useState, useEffect } from "react";
import CandidatePage from "./CandidatePage";
import VoterPage from "./VoterPage";
import VotingPage from "./VotingPage";

function Dashboard(): ReactElement {
  const [storedValue, setStoredValue] = useState(0);

  useEffect(() => {
    console.log("Vote added " + storedValue);
  }, [storedValue]);

  return (
    <>
      <div style={{ display: "flex" }}>
        <VoterPage storedValue={storedValue} />
        <CandidatePage storedValue={storedValue} />
      </div>
      <VotingPage handleChange={() => setStoredValue((count) => count + 1)} />
    </>
  );
}

export default Dashboard;
