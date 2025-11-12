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
import {
  FormikCustomCheckbox,
  FormikDatePicker,
} from 'containers/form-validations/FormikFields';
import moment from 'moment';

const RefillTrakingModal = ({
  modalOpen,
  toggleModal,
  mdFusionLabNoList,
  canisterNOList,
  productList,
  unitTypedropdownItem,
  refillCanSizeitem,
  refillDetail,
  onRefillSubmit,
}) => {
  const modalTitle =
    refillDetail === null || refillDetail === undefined
      ? 'Add Refill Traking'
      : 'Edit Refill Traking';
  const RefillTrakingSchema = Yup.object().shape({
    fusionLabNo: Yup.string().required('Fusion lab no is required!'),
    canisterNO: Yup.string().required('Canister no is required!'),
    // refillDate: Yup.date().nullable().required('Date required'),
    product: Yup.string().required('Product is required!'),
    refillML: Yup.string().required('Unit is required!'),
    refillCanSize: Yup.number().required('Refill bag amount is required'),
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
            refillDetail === null || refillDetail === undefined
              ? 0
              : refillDetail.id,
          fusionLabNo:
            refillDetail === null || refillDetail === undefined
              ? ''
              : refillDetail.fusionLabNo,
          canisterNO:
            refillDetail === null || refillDetail === undefined
              ? ''
              : refillDetail.canisterID,
          // refillDate: refillDetail === null || refillDetail === undefined ? '' : moment(refillDetail.dateFilled).toDate(),
          product:
            refillDetail === null || refillDetail === undefined
              ? ''
              : refillDetail.productID,
          refillML:
            refillDetail === null || refillDetail === undefined
              ? '1'
              : refillDetail.unitID,
          refillCanSize:
            refillDetail === null || refillDetail === undefined
              ? ''
              : refillDetail.quantity,
          isActive:
            refillDetail === null || refillDetail === undefined
              ? ''
              : refillDetail.isActive,
        }}
        validationSchema={RefillTrakingSchema}
        onSubmit={onRefillSubmit}
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
                                            <IntlMessages id="refillManage.lable.Fusion.Lab.No" />
                                        </span>
                                    </Label>
                                     */}
                  <div className="form-group has-float-label">
                    <select
                      name="fusionLabNo"
                      className="form-control"
                      value={values.fusionLabNo}
                      onChange={handleChange}
                      onBlur={handleBlur}
                    >
                      <option value="">Select Fusion Lab No</option>
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
                      name="canisterNO"
                      className="form-control"
                      value={values.canisterNO}
                      onChange={handleChange}
                      onBlur={handleBlur}
                    >
                      <option value="">Select Cannister No</option>
                      {canisterNOList.map((e) => {
                        return (
                          <option value={e.value} key={Math.random()}>
                            {e.label}
                          </option>
                        );
                      })}
                    </select>
                    <span>
                      <IntlMessages id="refillManage.lable.Canister.NO" />
                    </span>
                  </div>
                  {errors.canisterNO && touched.canisterNO ? (
                    <div className="invalid-feedback d-block">
                      {errors.canisterNO}
                    </div>
                  ) : null}
                </Colxx>
              </Row>
              {/* 
                            <Row className="mt-3">
                                <Colxx sm={12}>
                                    <Label className="form-group has-float-label">
                                        <FormikDatePicker
                                            name="refillDate"
                                            value={values.refillDate}
                                            // selected= {moment(dateString).toDate()
                                            onChange={setFieldValue}
                                            onBlur={setFieldTouched}
                                        />
                                        <span>
                                            <IntlMessages id="refillManage.lable.Date" />
                                        </span>
                                    </Label>
                                    {errors.refillDate && touched.refillDate ? (
                                        <div className="invalid-feedback d-block">
                                            {errors.refillDate}
                                        </div>
                                    ) : null}
                                </Colxx>
                            </Row> */}
              <Row className="mt-3">
                <Colxx sm={9}>
                  {/* <Label className="form-group has-float-label">
                                        <Field
                                            className="form-control"
                                            name="refillCanSize"
                                            type="number"
                                        />
                                        <span>
                                            <IntlMessages id="refillManage.lable.quantity" />
                                        </span>
                                    </Label> */}
                  <div className="form-group has-float-label">
                    <select
                      name="refillCanSize"
                      className="form-control"
                      value={values.refillCanSize}
                      onChange={handleChange}
                      onBlur={handleBlur}
                    >
                      <option value="">Select Refill bag amount</option>
                      {refillCanSizeitem.reminder.map((e) => {
                        return (
                          <option value={e.value} key={Math.random()}>
                            {e.label}
                          </option>
                        );
                      })}
                    </select>
                    <span>
                      <IntlMessages id="refillManage.lable.quantity" />
                    </span>
                  </div>

                  {errors.refillCanSize && touched.refillCanSize ? (
                    <div className="invalid-feedback d-block">
                      {errors.refillCanSize}
                    </div>
                  ) : null}
                </Colxx>
                <Colxx sm={3}>
                  <div className="form-group has-float-label">
                    <select
                      name="refillML"
                      className="form-control"
                      value={values.refillML}
                      onChange={handleChange}
                      onBlur={handleBlur}
                    >
                      <option value="">Select</option>
                      {unitTypedropdownItem.map((e) => {
                        if (e.value === 2) {
                          return (
                            <option value={e.value} key={Math.random()}>
                              {e.label}
                            </option>
                          );
                        }
                      })}
                    </select>
                    <span>
                      <IntlMessages id="refillManage.lable.ML" />
                    </span>
                  </div>
                  {errors.refillML && touched.refillML ? (
                    <div className="invalid-feedback d-block">
                      {errors.refillML}
                    </div>
                  ) : null}
                </Colxx>
              </Row>
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
                                            <option value="">Select product</option>
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

export default RefillTrakingModal;
