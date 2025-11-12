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

const CanisterLookupModal = ({
  modalOpen,
  toggleModal,
  categories,
  onCanisterLookupSubmit,
  canisterLookupDetail,
  canisterLookupData,
}) => {
  const modalTitle =
    canisterLookupDetail === null || canisterLookupDetail === undefined
      ? 'Add Canister Lookup'
      : 'Edit Canister Lookup';
  const SignupSchema = Yup.object().shape({
    canisterSKU: Yup.string().required('SKU is required!'),
    canisterName: Yup.string().required('Name is required!'),
    canisterCode: Yup.string()
      .required('Code is required!')
      .matches(/^[a-zA-Z0-9\s]+$/, 'Code is invalid '),
  });

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
          canisterLookupID:
            canisterLookupDetail === null || canisterLookupDetail === undefined
              ? 0
              : canisterLookupDetail.id,
          canisterCode:
            canisterLookupDetail === null || canisterLookupDetail === undefined
              ? ''
              : canisterLookupDetail.canisterCode,
          canisterSKU:
            canisterLookupDetail === null || canisterLookupDetail === undefined
              ? ''
              : canisterLookupDetail.sku,
          canisterName:
            canisterLookupDetail === null || canisterLookupDetail === undefined
              ? ''
              : canisterLookupDetail.name
              ? canisterLookupDetail.name.trim()
              : '',
          isActive:
            canisterLookupDetail === null || canisterLookupDetail === undefined
              ? ''
              : canisterLookupDetail.isActive,
        }}
        validationSchema={SignupSchema}
        onSubmit={onCanisterLookupSubmit}
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
                <Field
                  className="form-control"
                  name="canisterCode"
                  maxLength={27}
                />
                {errors.canisterCode && touched.canisterCode ? (
                  <div className="invalid-feedback d-block">
                    {errors.canisterCode}
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
                />
                {errors.canisterName && touched.canisterName ? (
                  <div className="invalid-feedback d-block">
                    {errors.canisterName}
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

export default CanisterLookupModal;
