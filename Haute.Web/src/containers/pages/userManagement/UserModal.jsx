import React from 'react';
import * as Yup from 'yup';
import {
  Button,
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
  Label,
  Row,
} from 'reactstrap';
import { Formik, Form, Field } from 'formik';
import { Colxx } from 'components/common/CustomBootstrap';
import IntlMessages from 'helpers/IntlMessages';
import { FormikCustomCheckbox } from 'containers/form-validations/FormikFields';

const UserModal = ({
  modalOpen,
  toggleModal,
  categories,
  onUserSubmit,
  userDetail,
  btnLoader,
}) => {
  const modalTitle =
    userDetail === null || userDetail === undefined ? 'Add User' : 'Edit User';
  const SignupSchema = Yup.object().shape({
    firstName: Yup.string()
      .required('First Name is required!')
      .matches(
        /^[a-zA-Z]+$/,
        'You can not add numeric value, space and special character.'
      ),
    lastName: Yup.string()
      .required('Last Name is required!')
      .matches(
        /^[a-zA-Z]+$/,
        'You can not add numeric value, space and special character.'
      ),
    userName: Yup.string().required('User Name is required!'),
    email: Yup.string()
      .email('Invalid email format')
      .required('Email Id is required!'),
    password: Yup.string().required('Password is required!'),
    confirmPassword: Yup.string()
      .required('Confirm password is required!')
      .oneOf([Yup.ref('password'), null], 'Passwords does not match'),
    userType: Yup.string().required('User type is required!'),
    phone: Yup.string()
      .required('Phone no is required!')
      .matches(/^[0-9]+$/, 'Phone is invalid '),
    consultantID: Yup.string().required('ConsultantId is required!'),
    mdFusionLabNo: Yup.string().required('MD Fusion Lab No is required!'),
    country: Yup.string().required('Country is required!'),
    shop: Yup.string().required('Shop is required!'),
    bachLotNo: Yup.string().required('Batch Lot No is required!'),
    userName: Yup.string().required('UserName is required!'),
  });

  // validation end
  return (
    <Modal
      isOpen={modalOpen}
      toggle={toggleModal}
      wrapClassName="modal-right"
      backdrop="static"
      size="md"
    >
      <ModalHeader toggle={toggleModal}>
        <IntlMessages id={modalTitle} />
      </ModalHeader>
      <Formik
        initialValues={{
          ID:
            userDetail === null || userDetail === undefined ? 0 : userDetail.id,
          firstName:
            userDetail === null || userDetail === undefined
              ? ''
              : userDetail.firstName,
          lastName:
            userDetail === null || userDetail === undefined
              ? ''
              : userDetail.lastName,
          userName:
            userDetail === null || userDetail === undefined
              ? ''
              : userDetail.userName,
          email:
            userDetail === null || userDetail === undefined
              ? ''
              : userDetail.email,
          password:
            userDetail === null || userDetail === undefined
              ? ''
              : userDetail.password,
          confirmPassword:
            userDetail === null || userDetail === undefined
              ? ''
              : userDetail.password,
          phone:
            userDetail === null || userDetail === undefined
              ? ''
              : userDetail.phone,
          consultantID:
            userDetail === null || userDetail === undefined
              ? ''
              : userDetail.consultantID,
          mdFusionLabNo:
            userDetail === null || userDetail === undefined
              ? ''
              : userDetail.mdFusionLabNo,
          country:
            userDetail === null || userDetail === undefined
              ? ''
              : userDetail.country,
          shop:
            userDetail === null || userDetail === undefined
              ? ''
              : userDetail.shop,
          bachLotNo:
            userDetail === null || userDetail === undefined
              ? ''
              : userDetail.bachLotNo,
          userType:
            userDetail === null || userDetail === undefined
              ? ''
              : userDetail.roleID,
          isActive:
            userDetail === null || userDetail === undefined
              ? ''
              : userDetail.isActive,
        }}
        validationSchema={SignupSchema}
        onSubmit={onUserSubmit}
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
              <Row>
                <Colxx sm={6}>
                  <Label className="form-group has-float-label">
                    <Field
                      className="form-control"
                      name="firstName"
                      maxLength={50}
                    />
                    <span>
                      <IntlMessages id="First Name" />
                      <span class="required">*</span>
                    </span>
                  </Label>
                  {errors.firstName && touched.firstName ? (
                    <div className="invalid-feedback d-block">
                      {errors.firstName}
                    </div>
                  ) : null}
                </Colxx>
                <Colxx sm={6}>
                  <Label className="form-group has-float-label">
                    <Field
                      className="form-control"
                      name="lastName"
                      maxLength={50}
                    />
                    <span>
                      <IntlMessages id="Last Name" />
                      <span class="required">*</span>
                    </span>
                  </Label>
                  {errors.lastName && touched.lastName ? (
                    <div className="invalid-feedback d-block">
                      {errors.lastName}
                    </div>
                  ) : null}
                </Colxx>
              </Row>
              <Row className="mt-3">
                <Colxx sm={12}>
                  <Label className="form-group has-float-label">
                    <Field
                      className="form-control"
                      name="userName"
                      maxLength={100}
                    />
                    <span>
                      <IntlMessages id="User Name" />
                      <span class="required">*</span>
                    </span>
                  </Label>
                  {errors.userName && touched.userName ? (
                    <div className="invalid-feedback d-block">
                      {errors.userName}
                    </div>
                  ) : null}
                </Colxx>
              </Row>
              <Row className="mt-3">
                <Colxx sm={12}>
                  <Label className="form-group has-float-label">
                    <Field className="form-control" type="email" name="email" />
                    <span>
                      <IntlMessages id="Email Id" />
                      <span class="required">*</span>
                    </span>
                  </Label>
                  {errors.email && touched.email ? (
                    <div className="invalid-feedback d-block">
                      {errors.email}
                    </div>
                  ) : null}
                </Colxx>
              </Row>
              {/* Start */}

              {modalTitle && modalTitle == 'Add User' && (
                <Row className="mt-3">
                  <Colxx sm={12}>
                    <Label className="form-group has-float-label">
                      <Field
                        className="form-control"
                        type="password"
                        name="password"
                        maxLength={8}
                      />
                      <span>
                        <IntlMessages id="Password" />
                        <span class="required">*</span>
                      </span>
                    </Label>
                    {errors.password && touched.password ? (
                      <div className="invalid-feedback d-block">
                        {errors.password}
                      </div>
                    ) : null}
                  </Colxx>
                </Row>
              )}

              {/* //End */}
              {modalTitle && modalTitle == 'Add User' && (
                <Row className="mt-3">
                  <Colxx sm={12}>
                    <Label className="form-group has-float-label">
                      <Field
                        className="form-control"
                        type="password"
                        name="confirmPassword"
                        maxLength={8}
                      />
                      <span>
                        <IntlMessages id="Confirm Password" />
                        <span class="required">*</span>
                      </span>
                    </Label>
                    {errors.confirmPassword && touched.confirmPassword ? (
                      <div className="invalid-feedback d-block">
                        {errors.confirmPassword}
                      </div>
                    ) : null}
                  </Colxx>
                </Row>
              )}
              <Row className="mt-3">
                <Colxx sm={6}>
                  <Label className="form-group has-float-label">
                    <Field
                      className="form-control"
                      type="tel"
                      name="consultantID"
                      maxLength={4}
                    />
                    <span>
                      <IntlMessages id="ConsultantID" />
                      <span class="required">*</span>
                    </span>
                  </Label>
                  {errors.consultantID && touched.consultantID ? (
                    <div className="invalid-feedback d-block">
                      {errors.consultantID}
                    </div>
                  ) : null}
                </Colxx>
                <Colxx sm={6}>
                  <Label className="form-group has-float-label">
                    <Field
                      className="form-control"
                      type="tel"
                      name="mdFusionLabNo"
                    />
                    <span>
                      <IntlMessages id="MD Fusion Lab No" />
                      <span class="required">*</span>
                    </span>
                  </Label>
                  {errors.mdFusionLabNo && touched.mdFusionLabNo ? (
                    <div className="invalid-feedback d-block">
                      {errors.mdFusionLabNo}
                    </div>
                  ) : null}
                </Colxx>
              </Row>
              <Row className="mt-3">
                <Colxx sm={12}>
                  <Label className="form-group has-float-label">
                    <Field className="form-control" type="tel" name="phone" />
                    <span>
                      <IntlMessages id="Phone No" />
                      <span class="required">*</span>
                    </span>
                  </Label>
                  {errors.phone && touched.phone ? (
                    <div className="invalid-feedback d-block">
                      {errors.phone}
                    </div>
                  ) : null}
                </Colxx>
              </Row>
              <Row className="mt-3">
                <Colxx sm={6}>
                  <Label className="form-group has-float-label">
                    <Field className="form-control" type="tel" name="country" />
                    <span>
                      <IntlMessages id="Country" />
                      <span class="required">*</span>
                    </span>
                  </Label>
                  {errors.country && touched.country ? (
                    <div className="invalid-feedback d-block">
                      {errors.country}
                    </div>
                  ) : null}
                </Colxx>
                <Colxx sm={6}>
                  <Label className="form-group has-float-label">
                    <Field className="form-control" type="tel" name="shop" />
                    <span>
                      <IntlMessages id="Shop" />
                      <span class="required">*</span>
                    </span>
                  </Label>
                  {errors.shop && touched.shop ? (
                    <div className="invalid-feedback d-block">
                      {errors.shop}
                    </div>
                  ) : null}
                </Colxx>
              </Row>

              <Row className="mt-3">
                <Colxx sm={6}>
                  <Label className="form-group has-float-label">
                    <Field
                      className="form-control"
                      type="tel"
                      name="bachLotNo"
                    />
                    <span>
                      <IntlMessages id="Batch Lot  No" />
                      <span class="required">*</span>
                    </span>
                  </Label>
                  {errors.bachLotNo && touched.bachLotNo ? (
                    <div className="invalid-feedback d-block">
                      {errors.bachLotNo}
                    </div>
                  ) : null}
                </Colxx>
                <Colxx sm={6}>
                  <div className="form-group has-float-label">
                    <select
                      name="userType"
                      className="form-control"
                      value={values.userType}
                      onChange={handleChange}
                      onBlur={handleBlur}
                    >
                      <option value="">Select User type</option>
                      {categories.map((e) => {
                        return (
                          <option value={e.value} key={Math.random()}>
                            {e.label}
                          </option>
                        );
                      })}
                    </select>
                    <span>
                      <IntlMessages id="User Type" />
                      <span class="required">*</span>
                    </span>
                  </div>
                  {errors.userType && touched.userType ? (
                    <div className="invalid-feedback d-block">
                      {errors.userType}
                    </div>
                  ) : null}
                </Colxx>
              </Row>

              <Row className="mt-3">
                <Colxx sm={6}>
                  <FormikCustomCheckbox
                    name="isActive"
                    value={values.isActive}
                    onChange={setFieldValue}
                    onBlur={setFieldTouched}
                    id="isActive"
                    label="Active?"
                  />
                </Colxx>
              </Row>
            </ModalBody>
            <ModalFooter>
              <Button
                outline
                onClick={toggleModal}
                color="primary"
                className="mb-2"
              >
                <IntlMessages id="Cancel" />
              </Button>
              <Button type="submit" color="primary" className="mb-2">
                <IntlMessages id="Submit" />
              </Button>
            </ModalFooter>
          </Form>
        )}
      </Formik>
    </Modal>
  );
};

export default UserModal;
