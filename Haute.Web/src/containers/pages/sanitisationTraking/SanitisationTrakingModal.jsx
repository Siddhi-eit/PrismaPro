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

const SanitisationTrakingModal = ({
  modalOpen,
  toggleModal,
  canisterNOList,
  dateSanitizedList,
  productList,
  onSanitisationSubmit,
  sanitisationDetail,
  mdFusionLabNoList,
}) => {
  const modalTitle =
    sanitisationDetail === null || sanitisationDetail === undefined
      ? 'Add Sanitisation Traking'
      : 'Edit Sanitisation Traking';
  const SanitisationTrakingSchema = Yup.object().shape({
    fusionLabNo: Yup.string().required('Fusion lab no is required!'),
    canisterID: Yup.string().required('A select option is required!'),
    // product: Yup.string().required('A select option is required!'),
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
          id:
            sanitisationDetail === null || sanitisationDetail === undefined
              ? 0
              : sanitisationDetail.id,
          fusionLabNo:
            sanitisationDetail === null || sanitisationDetail === undefined
              ? ''
              : sanitisationDetail.fusionLabNo,
          canisterID:
            sanitisationDetail === null || sanitisationDetail === undefined
              ? ''
              : sanitisationDetail.canisterID,
          // dateSanitized:
          //   sanitisationDetail === null || sanitisationDetail === undefined
          //     ? '10'
          //     : sanitisationDetail.dateSanitisedMonth,
          // product:
          //   sanitisationDetail === null || sanitisationDetail === undefined
          //     ? ''
          //     : sanitisationDetail.productID,
          isActive:
            sanitisationDetail === null || sanitisationDetail === undefined
              ? ''
              : sanitisationDetail.isActive,
        }}
        validationSchema={SanitisationTrakingSchema}
        onSubmit={onSanitisationSubmit}
      >
        {({
          setFieldValue,
          setFieldTouched,
          handleChange,
          handleBlur,
          values,
          errors,
          touched,
        }) => (
          <Form className="av-tooltip tooltip-label-bottom">
            <ModalBody>
              <Row>
                <Colxx sm={12}>
                  {/* <Label className="form-group has-float-label">
                                        <Field className="form-control" name="fusionLabNo" />
                                        <span>
                                            <IntlMessages id="lable.Fusion.Lab.No" />
                                        </span>
                                    </Label> */}
                  <div className="form-group has-float-label">
                    <select
                      name="fusionLabNo"
                      className="form-control"
                      value={values.fusionLabNo}
                      onChange={handleChange}
                      onBlur={handleBlur}
                    >
                      <option value="">Select an option..</option>
                      {mdFusionLabNoList.map((e) => {
                        return (
                          <option value={e.value} key={Math.random()}>
                            {e.label}
                          </option>
                        );
                      })}
                    </select>
                    <span>
                      <IntlMessages id="refillManage.lable.Fusion.Lab.No" />
                    </span>
                  </div>
                  {errors.fusionLabNo && touched.fusionLabNo ? (
                    <div className="invalid-feedback d-block">
                      {errors.fusionLabNo}
                    </div>
                  ) : null}
                </Colxx>
              </Row>
              <Row className="mt-3">
                <Colxx sm={12}>
                  <div className="form-group has-float-label">
                    <select
                      name="canisterID"
                      className="form-control"
                      value={values.canisterID}
                      onChange={handleChange}
                      onBlur={handleBlur}
                    >
                      <option value="">Select an option..</option>
                      {canisterNOList.map((e) => {
                        return (
                          <option value={e.value} key={Math.random()}>
                            {e.label}
                          </option>
                        );
                      })}
                    </select>
                    <span>
                      <IntlMessages id="sanitisation.lable.Canister.NO" />
                    </span>
                  </div>
                  {errors.canisterID && touched.canisterID ? (
                    <div className="invalid-feedback d-block">
                      {errors.canisterID}
                    </div>
                  ) : null}
                </Colxx>
              </Row>
              {/* <Row className="mt-3">
                <Colxx sm={12}>
                  <div className="form-group has-float-label">
                    <select
                      name="dateSanitized"
                      className="form-control"
                      value={values.dateSanitized}
                      onChange={handleChange}
                      onBlur={handleBlur}
                    >
                      {dateSanitizedList.map((e) => {
                        return (
                          <option value={e.value} key={Math.random()}>
                            {e.label}
                          </option>
                        );
                      })}
                    </select>
                    <span>
                      <IntlMessages id="sanitisation.lable.Date.sanitized" />
                    </span>
                  </div>
                </Colxx>
              </Row> */}
              {/* <Row className="mt-3">
                                <Colxx sm={12}>
                                    <div className="form-group has-float-label">
                                        <select
                                            name="product"
                                            className="form-control"
                                            value={values.product}
                                            onChange={handleChange}
                                            onBlur={handleBlur}
                                        >
                                            <option value="">Select an option..</option>
                                            {productList.map((e) => {
                                                return (
                                                    <option value={e.value} key={Math.random()}>
                                                        {e.label}
                                                    </option>
                                                );
                                            })}
                                        </select>
                                        <span>
                                            <IntlMessages id="sanitisation.lable.Product" />
                                        </span>
                                    </div>
                                    {errors.product && touched.product ? (
                                        <div className="invalid-feedback d-block">
                                            {errors.product}
                                        </div>
                                    ) : null}
                                </Colxx>
                            </Row> */}
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
              <Button color="primary" className="mb-2">
                <IntlMessages id="Submit" />
              </Button>
            </ModalFooter>
          </Form>
        )}
      </Formik>
    </Modal>
  );
};

export default SanitisationTrakingModal;
