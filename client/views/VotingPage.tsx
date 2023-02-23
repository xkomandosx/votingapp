import React from "react";
import dotnetify, { IDotnetifyVM } from "dotnetify";
import { ThemeProvider } from "@material-ui/core/styles";
import InputLabel from "@material-ui/core/InputLabel";
import MenuItem from "@material-ui/core/MenuItem";
import FormHelperText from "@material-ui/core/FormHelperText";
import FormControl from "@material-ui/core/FormControl";
import Select from "@material-ui/core/Select";
import Button from "@material-ui/core/Button";
import SendIcon from "@material-ui/icons/Send";

const styles = {
  addButton: { margin: "1em" },
  page: { marginLeft: "3em" },
  dropdown: { width: "200px", marginLeft: "3em" },
};

class Voter {
  Id: string;
  FirstName: string;
  LastName: string;
  Voted: boolean;
}

class Candidate {
  Id: string;
  FirstName: string;
  LastName: string;
  Votes: number;
}

class Voting {
  VotedFor: number;
  VotedBy: number;

  constructor(votedBy: number, votedFor: number) {
    this.VotedBy = votedBy;
    this.VotedFor = votedFor;
  }
}

class VotingPageModel {
  Voters: Voter[] = [];
  Candidates: Candidate[] = [];
  Add: number;
}

class VotingPageState extends VotingPageModel {
  votedFor: number;
  votedBy: number;
}

export default class VotingPage extends React.Component<any, VotingPageState> {
  state: VotingPageState = new VotingPageState();
  vm: IDotnetifyVM;
  dispatch: (state: any) => void;

  constructor(props: any) {
    super(props);
    this.vm = dotnetify.react.connect("VotingPage", this);
    this.dispatch = (state) => this.vm.$dispatch(state);
  }

  componentWillUnmount() {
    this.vm.$destroy();
  }

  render() {
    let { votedBy, votedFor, Voters, Candidates } = this.state;

    const handleAdd = () => {
      if (votedBy && votedFor) {
        let newState = new Voting(votedBy, votedFor);
        this.dispatch({ Add: newState });
        this.setState({ votedBy: null });
        this.setState({ votedFor: null });
        this.props.handleChange(1);
      }
    };
    const handleVoterChange = (
      event: React.ChangeEvent<{ value: unknown }>
    ) => {
      this.setState({ votedBy: event.target.value as number });
    };
    const handleCandidateChange = (
      event: React.ChangeEvent<{ value: unknown }>
    ) => {
      this.setState({ votedFor: event.target.value as number });
    };

    return (
      <div style={styles.page}>
        <FormControl style={styles.dropdown}>
          <InputLabel id="voted-by-label">I am</InputLabel>
          <Select
            labelId="voted-by"
            id="voted-by"
            value={votedBy ? votedBy : ""}
            label="Voter *"
            onChange={handleVoterChange}
          >
            <MenuItem value="">
              <em>None</em>
            </MenuItem>
            {Voters.map((item, index) => (
              <MenuItem key={index} value={item.Id}>
                {item.FirstName} {item.LastName}
              </MenuItem>
            ))}
          </Select>
          <FormHelperText>Required</FormHelperText>
        </FormControl>
        <FormControl style={styles.dropdown}>
          <InputLabel id="voted-for-label">I voted for</InputLabel>
          <Select
            labelId="voted-for"
            id="voted-for"
            value={votedFor ? votedFor : ""}
            label="Candidate *"
            onChange={handleCandidateChange}
          >
            <MenuItem value="">
              <em>None</em>
            </MenuItem>
            {Candidates.map((item, index) => (
              <MenuItem key={index} value={item.Id}>
                {item.FirstName} {item.LastName}
              </MenuItem>
            ))}
          </Select>
          <FormHelperText>Required</FormHelperText>
        </FormControl>
        <Button
          variant="contained"
          endIcon={<SendIcon />}
          onClick={handleAdd}
          style={styles.addButton}
        >
          Submit!
        </Button>
      </div>
    );
  }
}
