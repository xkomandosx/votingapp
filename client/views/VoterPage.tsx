import React from "react";
import dotnetify, { IDotnetifyVM } from "dotnetify";
import { ThemeProvider } from "@material-ui/core/styles";
import Fab from "@material-ui/core/Fab";
import Snackbar from "@material-ui/core/Snackbar";
import TextField from "@material-ui/core/TextField";
import Table from "@material-ui/core/Table";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import IconButton from "@material-ui/core/IconButton";
import DeleteIcon from "@material-ui/icons/Delete";
import CheckBoxIcon from "@material-ui/icons/CheckBox";
import CheckBoxOutlineBlankIcon from "@material-ui/icons/CheckBoxOutlineBlank";
import AddIcon from "@material-ui/icons/Add";
import BasePage from "../components/BasePage";
import Pagination from "../components/table/Pagination";
import InlineEdit from "../components/table/InlineEdit";
import defaultTheme from "../styles/theme-default";

const styles = {
  addButton: { margin: "1em" },
  columns: {
    id: { width: "10%" },
    firstName: { width: "25%" },
    lastName: { width: "25%" },
    voted: { width: "20%" },
    remove: { width: "15%" },
  },
  pagination: { marginTop: "1em" },
};

class Voter {
  Id: string;
  FirstName: string;
  LastName: string;
  Voted: boolean;
}

class VoterPageModel {
  Voters: Voter[] = [];
  Pages: number[] = [];
  SelectedPage: number;
  ShowNotification: boolean;
  Add: string;
}

class VoterPageState extends VoterPageModel {
  addName: string = "";
}

export default class VoterPage extends React.Component<any, VoterPageState> {
  state: VoterPageState = new VoterPageState();
  vm: IDotnetifyVM;
  dispatch: (state: any) => void;

  constructor(props: any) {
    super(props);
    this.vm = dotnetify.react.connect("VoterTable", this);
    this.dispatch = (state) => this.vm.$dispatch(state);
  }

  componentWillUnmount() {
    this.vm.$destroy();
  }

  componentDidUpdate(prevProps) {
    if (prevProps.storedValue !== this.props.storedValue) {
      this.vm.$destroy();
      this.vm = dotnetify.react.connect("VoterTable", this);
      this.dispatch = (state) => this.vm.$dispatch(state);
    }
  }

  render() {
    let { addName, Voters, Pages, SelectedPage, ShowNotification } = this.state;

    const handleAdd = () => {
      if (addName) {
        this.dispatch({ Add: addName });
        this.setState({ addName: "" });
      }
    };

    const handleUpdate = (voter: {
      Id: string;
      FirstName?: string;
      LastName?: string;
      Voted?: boolean;
    }) => {
      let newState = Voters.map((item) =>
        item.Id === voter.Id ? Object.assign(item, voter) : item
      );
      this.setState({ Voters: newState });
      this.dispatch({ Update: voter });
    };

    const handleSelectPage = (page: number) => {
      const newState = { SelectedPage: page };
      this.setState(newState);
      this.dispatch(newState);
    };

    const hideNotification = (_) => this.setState({ ShowNotification: false });

    return (
      <ThemeProvider theme={defaultTheme}>
        <BasePage title="Voters" navigation="">
          <div>
            <div>
              <Fab
                onClick={handleAdd}
                style={styles.addButton}
                color="secondary"
              >
                <AddIcon />
              </Fab>
              <TextField
                id="AddName"
                label="Add"
                helperText="Type full name here"
                value={addName}
                onKeyPress={(event) =>
                  event.key === "Enter" ? handleAdd() : null
                }
                onChange={(event) =>
                  this.setState({ addName: event.target.value })
                }
              />
            </div>

            <Table>
              <TableHead>
                <TableRow>
                  <TableCell style={styles.columns.id}>ID</TableCell>
                  <TableCell style={styles.columns.firstName}>
                    First Name
                  </TableCell>
                  <TableCell style={styles.columns.lastName}>
                    Last Name
                  </TableCell>
                  <TableCell style={styles.columns.voted}>Voted</TableCell>
                  <TableCell style={styles.columns.remove}>Remove</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {Voters.map((item) => (
                  <TableRow key={item.Id}>
                    <TableCell style={styles.columns.id}>{item.Id}</TableCell>
                    <TableCell style={styles.columns.firstName}>
                      <InlineEdit
                        onChange={(value) =>
                          handleUpdate({ Id: item.Id, FirstName: value })
                        }
                      >
                        {item.FirstName}
                      </InlineEdit>
                    </TableCell>
                    <TableCell style={styles.columns.lastName}>
                      <InlineEdit
                        onChange={(value) =>
                          handleUpdate({ Id: item.Id, LastName: value })
                        }
                      >
                        {item.LastName}
                      </InlineEdit>
                    </TableCell>
                    <TableCell style={styles.columns.voted}>
                      {item.Voted ? (
                        <CheckBoxIcon />
                      ) : (
                        <CheckBoxOutlineBlankIcon />
                      )}
                    </TableCell>
                    <TableCell style={styles.columns.remove}>
                      <IconButton
                        onClick={(_) => this.dispatch({ Remove: item.Id })}
                      >
                        <DeleteIcon />
                      </IconButton>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>

            <Pagination
              style={styles.pagination}
              pages={Pages}
              select={SelectedPage}
              onSelect={handleSelectPage}
            />

            <Snackbar
              open={ShowNotification}
              message="Changes saved"
              autoHideDuration={1000}
              onClose={hideNotification}
            />
          </div>
        </BasePage>
      </ThemeProvider>
    );
  }
}
