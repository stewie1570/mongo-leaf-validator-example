import React, { useState, useEffect } from "react";
import {
  Leaf,
  useValidationModel,
  useLoadingState,
  leafDiff,
  set,
  useErrorHandler,
} from "leaf-validator";
import { TextInput } from "./components/TextInput";
import axios from "axios";

import "./custom.css";

const isRequired = (value) =>
  (!value || value.trim() === "") && ["Value is required"];
const isValidEmailAddress = (value) =>
  !/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(value) && [
    `"${value || ""}" is not a valid email address`,
  ];
const isValidPhoneNumber = (value) =>
  !/^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/.test(value) && [
    `"${value || ""}" is not a valid phone number`,
  ];

const form = [
  {
    name: "First Name",
    location: "person.firstName",
    validators: [isRequired],
  },
  {
    name: "Last Name",
    location: "person.lastName",
    validators: [isRequired],
  },
  {
    name: "Email",
    location: "person.contact.email",
    validators: [isRequired, isValidEmailAddress],
  },
  {
    name: "Phone Number",
    location: "person.voiceContact.phoneNumber",
    failOverLocations: ["person.contact.phoneNumber"],
    validators: [isRequired, isValidPhoneNumber],
  },
  {
    name: "Favorite Number",
    location: "person.favoriteNumber",
    inputProps: { isFloat: true },
  },
];

export function App() {
  const [originalModel, setOriginalModel] = useState();
  const [model, setModel] = useState();
  const validationModel = useValidationModel();
  const [showAllValidation, setShowAllValidation] = useState(false);
  const [isSubmitting, showSubmittingWhile] = useLoadingState();
  const [isLoadingModel, showLoadingModelWhile] = useLoadingState();
  const { clearError, errorHandler, errors } = useErrorHandler();
  const submit = async (event) => {
    event.preventDefault();
    setShowAllValidation(true);
    if (validationModel.getAllErrorsForLocation().length === 0) {
      await errorHandler(
        showSubmittingWhile(
          axios
            .post("/Contact", leafDiff.from(originalModel).to(model))
            .catch(() => {})
            .then(loadCurrentModel)
        )
      );
    }
  };

  const loadCurrentModel = async () => {
    const model = (await axios.get("/Contact")).data;
    setModel(model);
    setOriginalModel(model);
  };

  useEffect(() => {
    showLoadingModelWhile(loadCurrentModel());
    // eslint-disable-next-line
  }, []);

  return isLoadingModel ? (
    <i>Loading...</i>
  ) : !model ? (
    <></>
  ) : (
    <div className="App">
      {errors?.length > 0 && (
        <ul>
          {errors.map((error) => (
            <li key={error.message}>
              <button
                className="btn btn-danger"
                onClick={() => clearError(error)}
              >
                X
              </button>
              {error.message}
            </li>
          ))}
        </ul>
      )}
      <form>
        {form.map(({ name, inputProps, ...formElement }, index) => (
          <div
            key={index}
            style={{
              width: "200px",
              display: "inline-block",
              paddingLeft: "10px",
              verticalAlign: "top",
            }}
          >
            <Leaf
              showErrors={showAllValidation}
              model={model}
              onChange={setModel}
              validationModel={validationModel}
              {...formElement}
            >
              {(value, onChange, onBlur, errors) => (
                <label>
                  {name}
                  <TextInput
                    {...inputProps}
                    value={value}
                    onChange={onChange}
                    onBlur={onBlur}
                    className={`${
                      errors.length > 0 ? "is-invalid " : ""
                    }form-control mb-1`}
                  />
                  {errors.length > 0 && (
                    <ul className="errors">
                      {errors.map((error, index) => (
                        <li
                          data-testid="error"
                          style={{ overflowWrap: "anywhere" }}
                          key={index}
                        >
                          {error}
                        </li>
                      ))}
                    </ul>
                  )}
                </label>
              )}
            </Leaf>
          </div>
        ))}
        <br />
        &nbsp;
        <br />
        <button
          disabled={isSubmitting}
          className="btn btn-primary"
          type="submit"
          onClick={submit}
          style={{ marginLeft: "10px" }}
        >
          {isSubmitting ? "Submitting..." : "Submit"}
        </button>
      </form>
      <br />
      &nbsp;
      <br />
      <button
        className="btn btn-secondary"
        onClick={() =>
          setModel(set("person.favoriteNumber").to(undefined).in(model))
        }
        style={{ marginLeft: "10px" }}
      >
        Delete Favorite #
      </button>
    </div>
  );
}
