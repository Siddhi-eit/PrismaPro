import React from 'react';
import * as Yup from 'yup';
import {
  Button,
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
  Label,
  FormGroup,
} from 'reactstrap';
import { Formik, Form, Field } from 'formik';
import IntlMessages from 'helpers/IntlMessages';
import { FormikCustomCheckbox } from 'containers/form-validations/FormikFields';
import { reset } from 'mousetrap';

const FormulaModal = ({
  modalOpen,
  toggleModal,
  formulaDetail,
  onFormulaSubmit,
  canisterLookupData,
}) => {
  const modalTitle =
    formulaDetail === null || formulaDetail === undefined
      ? 'Add Formula'
      : 'Edit Formula';

  const SignupSchema = Yup.object().shape({
    productCode: Yup.string().required('ProductCode is required!'),
    // .matches(/^[a-zA-Z0-9\s]+$/, 'ProductCode is invalid '),
    dispenseAmount: Yup.string()
      .required('DispenseAmount is required!')
      .matches(/^[a-zA-Z0-9\s]+$/, 'DispenseAmount is invalid '),
    canisterData: Yup.array()
      .of(
        Yup.object().shape({
          colorCode: Yup.string().required('Color Code is required!'),
          // canisterLookupId: Yup.string().required('Code is required!'),
          //   .matches(/^[a-zA-Z0-9\s]+$/, 'Code is invalid '),
          amount: Yup.string().required('Amount is required!'),
          // .matches(/^[a-zA-Z0-9\s]+$/, 'Amount is invalid '),
        })
      )
      .required('At least one canister is required.'),
  });

  const initialCanisterData = [
    {
      canisterLookupId: '',
      amount: '',
    },
  ];
  console.log(formulaDetail);

  const handleSubmit = (values, { resetForm }) => {
    // console.log('Submitting values: ', values);
    // onFormulaSubmit(values);
    // resetForm(); // Reset the form after submission

    const { canisterData, ...rest } = values;

    // Transform canisterData to only include amounts
    const transformedCanisterData = canisterData.map(
      ({ amount, colorCode }) => ({
        amount: String(amount), // Convert amount to a string
        colorCode,
      })
    );

    const finalValues = {
      formulaID: values.formulaID,
      productCode: values.productCode, // Example: Populate the formula field with productCode
      dispenseAmount: String(values.dispenseAmount), // Convert to string
      canisterData: transformedCanisterData,
    };

    console.log('Submitting values: ', finalValues);
    onFormulaSubmit(finalValues);
    resetForm(); //
  };
  return (
    <Modal
      isOpen={modalOpen}
      toggle={toggleModal}
      wrapClassName="modal-right"
      backdrop="static"
    >
      <ModalHeader toggle={toggleModal}>
        <IntlMessages id={modalTitle} />
      </ModalHeader>
      <Formik
        initialValues={{
          formulaID: formulaDetail?.id || 0,
          productCode: formulaDetail?.productCode || '', // Ensure this is not undefined
          dispenseAmount: formulaDetail?.dispenseAmount || '',
          canisterData:
            formulaDetail?.colorAmounts?.map((item) => ({
              colorCode: item.colorCode.trim(),
              amount: item.amount,
            })) || initialCanisterData,
        }}
        validationSchema={SignupSchema}
        onSubmit={handleSubmit}
      >
        {({ values, errors, touched, setFieldValue }) => {
          // Log values, errors, and touched to the console for debugging
          console.log('Values:', values);
          console.log('Errors:', errors);
          console.log('Touched:', touched);

          return (
            <Form className="av-tooltip tooltip-label-bottom">
              <ModalBody>
                <FormGroup className="form-group has-float-label">
                  <Label>
                    <IntlMessages id="ProductCode" />
                  </Label>
                  <Field
                    className="form-control"
                    name="productCode"
                    maxLength={50}
                  />
                  {errors.productCode && touched.productCode ? (
                    <div className="invalid-feedback d-block">
                      {errors.productCode}
                    </div>
                  ) : null}
                </FormGroup>

                <FormGroup className="form-group has-float-label mt-5">
                  <Label>
                    <IntlMessages id="DispenseAmount" />
                  </Label>
                  <Field
                    className="form-control"
                    name="dispenseAmount"
                    maxLength={100}
                  />
                  {errors.dispenseAmount && touched.dispenseAmount ? (
                    <div className="invalid-feedback d-block">
                      {errors.dispenseAmount}
                    </div>
                  ) : null}
                </FormGroup>

                {({ values, errors, touched, setFieldValue }) => {
                  // Log values, errors, and touched to the console for debugging
                  console.log('Values:', values);
                  console.log('Errors:', errors);
                  console.log('Touched:', touched);

                  return (
                    <Form className="av-tooltip tooltip-label-bottom">
                      <ModalBody>
                        {/* Your form fields and components go here */}
                      </ModalBody>
                    </Form>
                  );
                }}

                {(values.canisterData || []).map((_, index) => (
                  <div
                    key={index}
                    className="form-color-code row position-relative pb-2"
                  >
                    <FormGroup className="form-group has-float-label mt-5 col-6">
                      <Label>
                        <IntlMessages id="Color Code" />
                      </Label>
                      <Field
                        as="select"
                        name={`canisterData[${index}].colorCode`}
                        className="form-control"
                      >
                        <option value="">Select Canister Code ..</option>
                        {canisterLookupData &&
                          canisterLookupData.map((e) => (
                            <option
                              value={e.canisterCode.trim()}
                              key={e.canisterCode.trim()}
                            >
                              {e.canisterCode.trim()}
                            </option>
                          ))}
                      </Field>
                      {errors.canisterData &&
                      errors.canisterData[index] &&
                      errors.canisterData[index].colorCode &&
                      touched.canisterData &&
                      touched.canisterData[index] &&
                      touched.canisterData[index].canisterLookupId ? (
                        <div className="invalid-feedback d-block">
                          {errors.canisterData[index].colorCode}
                        </div>
                      ) : null}
                    </FormGroup>

                    <FormGroup className="form-group has-float-label mt-5 col-6 ">
                      <Label>
                        <IntlMessages id="Amount" />
                      </Label>
                      <Field
                        className="form-control"
                        name={`canisterData[${index}].amount`}
                        maxLength={50}
                      />
                      {errors.canisterData &&
                      errors.canisterData[index] &&
                      errors.canisterData[index].amount &&
                      touched.canisterData &&
                      touched.canisterData[index] &&
                      touched.canisterData[index].amount ? (
                        <div className="invalid-feedback d-block">
                          {errors.canisterData[index].amount}
                        </div>
                      ) : null}
                      {index > 0 && (
                        <Button
                          color=""
                          onClick={() => {
                            setFieldValue(
                              'canisterData',
                              values.canisterData.filter((_, i) => i !== index)
                            );
                          }}
                          className="position-absolute array-button"
                        >
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            width="20"
                            height="20"
                            fill="red"
                            class="bi bi-dash-circle"
                            viewBox="0 0 16 16"
                          >
                            <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14m0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16" />
                            <path d="M4 8a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7A.5.5 0 0 1 4 8" />
                          </svg>
                        </Button>
                      )}
                    </FormGroup>

                    {/* Minus Button for Removing This Entry, only display if index > 0 */}
                  </div>
                ))}

                <div className="add-canister-button text-center">
                  <Button
                    color="primary"
                    onClick={() =>
                      setFieldValue('canisterData', [
                        ...values.canisterData,
                        { canisterLookupId: '', amount: '' },
                      ])
                    }
                    className="mt-4"
                  >
                    {/* <i className="simple-icon-plus btn-group-icon w-50" /> */}
                    ADD MORE COLORCODE
                  </Button>
                </div>
              </ModalBody>
              <ModalFooter>
                <Button
                  outline
                  onClick={() => {
                    toggleModal();
                  }}
                  color="primary"
                >
                  <IntlMessages id="Cancel" />
                </Button>
                <Button color="primary" type="submit">
                  <IntlMessages id="Submit" />
                </Button>{' '}
              </ModalFooter>
            </Form>
          );
        }}
      </Formik>
    </Modal>
  );
};

export default FormulaModal;
