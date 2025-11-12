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
// import Select from 'react-select';
// import CustomSelectInput from 'components/common/CustomSelectInput';
import IntlMessages from 'helpers/IntlMessages';
import { FormikCustomCheckbox } from 'containers/form-validations/FormikFields';

const CanisterModal = ({
  modalOpen,
  toggleModal,
  categories,
  onCanisterSubmit,
  canisterDetail,
  canisterLookupData,
}) => {
  const modalTitle =
    canisterDetail === null || canisterDetail === undefined
      ? 'Add Canister'
      : 'Edit Canister';
  const SignupSchema = Yup.object().shape({
    canisterSelect: Yup.string().required('A select option is required!'),
    canisterLookupId: Yup.string()
      .required('Code is required!')
      .matches(/^[a-zA-Z0-9\s]+$/, 'Code is invalid '),
    maximumAmount: Yup.number()
      .required('Maximum amount is required')
      .test(
        'Is positive?',
        'The number must be greater than 0!',
        (value) => value > 0
      ),
    currentAmount: Yup.number()
      .required('Current amount is required')
      .test(
        'Is positive?',
        'The number must be greater than 0!',
        (value) => value > 0
      ),
    minimumAmount: Yup.number()
      .required('Minimum amount is required')
      .test(
        'Is positive?',
        'The number must be greater than 0!',
        (value) => value > 0
      ),
    warningAmount: Yup.number()
      .required('Warning amount is required')
      .test(
        'Is positive?',
        'The number must be greater than 0!',
        (value) => value > 0
      ),
  });

  const onCanisterCodeChange = (value, setFieldValue) => {
    let canisterData =
      canisterLookupData &&
      canisterLookupData.find((data) => Number(data.id) == Number(value));
    setFieldValue(
      'canisterSKU',
      canisterData && canisterData.sku ? canisterData.sku : ''
    );
    setFieldValue(
      'canisterName',
      canisterData && canisterData.name ? canisterData.name : ''
    );
  };

  // validation end
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
          canisterID:
            canisterDetail === null || canisterDetail === undefined
              ? 0
              : canisterDetail.id,
          canisterLookupId:
            canisterDetail === null || canisterDetail === undefined
              ? ''
              : canisterDetail.canisterLookupId.toString(),
          canisterSKU:
            canisterDetail === null || canisterDetail === undefined
              ? ''
              : canisterDetail.sku,
          canisterName:
            canisterDetail === null || canisterDetail === undefined
              ? ''
              : canisterDetail.name.trim(),
          maximumAmount:
            canisterDetail === null || canisterDetail === undefined
              ? ''
              : canisterDetail.maximumAmount,
          minimumAmount:
            canisterDetail === null || canisterDetail === undefined
              ? ''
              : canisterDetail.minimumAmount,
          currentAmount:
            canisterDetail === null || canisterDetail === undefined
              ? ''
              : canisterDetail.currentAmount,
          warningAmount:
            canisterDetail === null || canisterDetail === undefined
              ? ''
              : canisterDetail.warningAmount,
          canisterSelect:
            canisterDetail === null || canisterDetail === undefined
              ? '1'
              : canisterDetail.unitID,
          isActive:
            canisterDetail === null || canisterDetail === undefined
              ? ''
              : canisterDetail.isActive,
        }}
        validationSchema={SignupSchema}
        onSubmit={onCanisterSubmit}
      >
        {({
          setFieldValue,
          setFieldTouched,
          handleBlur,
          handleChange,
          values,
          errors,
          touched,
        }) => (
          <Form className="av-tooltip tooltip-label-bottom">
            <ModalBody>
              <FormGroup className="form-group has-float-label">
                <Label>
                  <IntlMessages id="Code" />
                </Label>
                <select
                  name="canisterLookupId"
                  className="form-control"
                  value={values.canisterLookupId}
                  onChange={(event) => {
                    onCanisterCodeChange(event.target.value, setFieldValue);
                    handleChange(event);
                  }}
                  onBlur={handleBlur}
                >
                  <option value="">Select Canister Code ..</option>
                  {canisterLookupData.map((e) => {
                    return (
                      <option value={e.id} key={Math.random()}>
                        {e.canisterCode}
                      </option>
                    );
                  })}
                </select>
                {errors.canisterLookupId && touched.canisterLookupId ? (
                  <div className="invalid-feedback d-block">
                    {errors.canisterLookupId}
                  </div>
                ) : null}
              </FormGroup>
              <FormGroup className="form-group has-float-label mt-5">
                <Label>
                  <IntlMessages id="SKU" />
                </Label>
                <Field
                  className="form-control"
                  name="canisterSKU"
                  maxLength={27}
                  disabled
                />
                {errors.canisterSKU && touched.canisterSKU ? (
                  <div className="invalid-feedback d-block">
                    {errors.canisterSKU}
                  </div>
                ) : null}
              </FormGroup>
              <FormGroup className="form-group has-float-label mt-5">
                <Label>
                  <IntlMessages id="Name" />
                </Label>
                <Field
                  className="form-control"
                  name="canisterName"
                  maxLength={50}
                  disabled
                />
                {errors.canisterName && touched.canisterName ? (
                  <div className="invalid-feedback d-block">
                    {errors.canisterName}
                  </div>
                ) : null}
              </FormGroup>
              <FormGroup className="form-group has-float-label mt-5">
                <Label>
                  <IntlMessages id="Dispense unit" />
                </Label>
                <select
                  name="canisterSelect"
                  className="form-control"
                  value={values.canisterSelect}
                  onChange={handleChange}
                  onBlur={handleBlur}
                  disabled
                >
                  {/* <option value="">Select an option..</option> */}
                  {categories.map((e) => {
                    return (
                      <option value={e.value} key={Math.random()}>
                        {e.label}
                      </option>
                    );
                  })}
                </select>
                {errors.canisterSelect && touched.canisterSelect ? (
                  <div className="invalid-feedback d-block">
                    {errors.canisterSelect}
                  </div>
                ) : null}
              </FormGroup>
              <FormGroup className="form-group has-float-label mt-5">
                <Label>
                  <IntlMessages id="Current Amount" />
                </Label>
                <Field
                  className="form-control"
                  name="currentAmount"
                  type="number"
                />
                {errors.currentAmount && touched.currentAmount ? (
                  <div className="invalid-feedback d-block">
                    {errors.currentAmount}
                  </div>
                ) : null}
              </FormGroup>
              <FormGroup className="form-group has-float-label mt-5">
                <Label>
                  <IntlMessages id="Maximum Amount" />
                </Label>
                <Field
                  className="form-control"
                  name="maximumAmount"
                  type="number"
                />
                {errors.maximumAmount && touched.maximumAmount ? (
                  <div className="invalid-feedback d-block">
                    {errors.maximumAmount}
                  </div>
                ) : null}
              </FormGroup>

              <FormGroup className="form-group has-float-label mt-5">
                <Label>
                  <IntlMessages id="Minimum Amount" />
                </Label>
                <Field
                  className="form-control"
                  name="minimumAmount"
                  type="number"
                />
                {errors.minimumAmount && touched.minimumAmount ? (
                  <div className="invalid-feedback d-block">
                    {errors.minimumAmount}
                  </div>
                ) : null}
              </FormGroup>
              <FormGroup className="form-group has-float-label mt-5">
                <Label>
                  <IntlMessages id="Warning Amount" />
                </Label>
                <Field
                  className="form-control"
                  name="warningAmount"
                  type="number"
                />
                {errors.warningAmount && touched.warningAmount ? (
                  <div className="invalid-feedback d-block">
                    {errors.warningAmount}
                  </div>
                ) : null}
              </FormGroup>

              <FormGroup className="error-l-150 mt-4">
                <FormikCustomCheckbox
                  name="isActive"
                  value={values.isActive}
                  onChange={setFieldValue}
                  onBlur={setFieldTouched}
                  label="Active?"
                  id="isActive"
                />
              </FormGroup>
            </ModalBody>
            <ModalFooter>
              <Button outline onClick={toggleModal} color="primary">
                <IntlMessages id="Cancel" />
              </Button>
              <Button color="primary">
                <IntlMessages id="Submit" />
              </Button>{' '}
            </ModalFooter>
          </Form>
        )}
      </Formik>
    </Modal>
  );
};

export default CanisterModal;
