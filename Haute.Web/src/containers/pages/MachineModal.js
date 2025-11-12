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

const MachineModal = ({
  modalOpen,
  toggleModal,
  onMachineSubmit,
  machineDetail,
}) => {
  const modalTitle =
    machineDetail === null || machineDetail === undefined
      ? 'Add Machine'
      : 'Edit Machine';
  const SignupSchema = Yup.object().shape({
    machineRegNo: Yup.string()
      .required('MachineRegNo is required!')
      .matches(/^[a-zA-Z0-9\s]+$/, 'MachineRegNo is invalid '),
    shopName: Yup.string()
      .required('ShopName is required!')
      .matches(/^[a-zA-Z0-9\s]+$/, 'ShopName is invalid '),
    city: Yup.string()
      .required('City is required!')
      .matches(/^[a-zA-Z0-9\s]+$/, 'City is invalid '),
    state: Yup.string()
      .required('State is required!')
      .matches(/^[a-zA-Z0-9\s]+$/, 'State is invalid '),
    macAddress: Yup.string().required('MacAddress in required!'),
  });

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
          machineID:
            machineDetail === null || machineDetail === undefined
              ? 0
              : machineDetail.id,
          machineRegNo:
            machineDetail === null || machineDetail === undefined
              ? ''
              : machineDetail.machineRegNo,
          shopName:
            machineDetail === null || machineDetail === undefined
              ? ''
              : machineDetail.shopName,
          shopAddress:
            machineDetail === null || machineDetail === undefined
              ? ''
              : machineDetail.shopAddress,
          city:
            machineDetail === null || machineDetail === undefined
              ? ''
              : machineDetail.city,
          state:
            machineDetail === null || machineDetail === undefined
              ? ''
              : machineDetail.state,
          isActive:
            machineDetail === null || machineDetail === undefined
              ? ''
              : machineDetail.isActive,
          macAddress:
            machineDetail === null || machineDetail === undefined
              ? ''
              : machineDetail.macAddress
              ? machineDetail.macAddress
              : '',
        }}
        validationSchema={SignupSchema}
        onSubmit={onMachineSubmit}
      >
        {({ setFieldValue, setFieldTouched, values, errors, touched }) => (
          <Form className="av-tooltip tooltip-label-bottom">
            <ModalBody>
              <FormGroup className="form-group has-float-label">
                <Label>
                  <IntlMessages id="MachineRegNo" />
                </Label>
                <Field
                  className="form-control"
                  name="machineRegNo"
                  maxLength={50}
                />
                {errors.machineRegNo && touched.machineRegNo ? (
                  <div className="invalid-feedback d-block">
                    {errors.machineRegNo}
                  </div>
                ) : null}
              </FormGroup>
              <FormGroup className="form-group has-float-label mt-5">
                <Label>
                  <IntlMessages id="ShopName" />
                </Label>
                <Field
                  className="form-control"
                  name="shopName"
                  maxLength={100}
                />
                {errors.shopName && touched.shopName ? (
                  <div className="invalid-feedback d-block">
                    {errors.shopName}
                  </div>
                ) : null}
              </FormGroup>
              <FormGroup className="form-group has-float-label mt-5">
                <Label>
                  <IntlMessages id="ShopAddress" />
                </Label>
                <Field
                  className="form-control"
                  name="shopAddress"
                  maxLength={50}
                />
                {errors.shopAddress && touched.shopAddress ? (
                  <div className="invalid-feedback d-block">
                    {errors.shopAddress}
                  </div>
                ) : null}
              </FormGroup>
              <FormGroup className="form-group has-float-label mt-5">
                <Label>
                  <IntlMessages id="City" />
                </Label>
                <Field className="form-control" name="city" maxLength={50} />
                {errors.city && touched.city ? (
                  <div className="invalid-feedback d-block">{errors.city}</div>
                ) : null}
              </FormGroup>
              <FormGroup className="form-group has-float-label mt-5">
                <Label>
                  <IntlMessages id="State" />
                </Label>
                <Field className="form-control" name="state" maxLength={50} />
                {errors.state && touched.state ? (
                  <div className="invalid-feedback d-block">{errors.state}</div>
                ) : null}
              </FormGroup>
              <FormGroup className="form-group has-float-label mt-5">
                <Label>
                  <IntlMessages id="MacAddress" />
                </Label>
                <Field
                  className="form-control"
                  name="macAddress"
                  maxLength={100}
                />
                {errors.macAddress && touched.macAddress ? (
                  <div className="invalid-feedback d-block">
                    {errors.macAddress}
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

export default MachineModal;
