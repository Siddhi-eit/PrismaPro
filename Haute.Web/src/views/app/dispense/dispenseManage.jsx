import React, { useEffect, useState } from 'react';
import {
  Button,
  Card,
  CardBody,
  CardTitle,
  FormGroup,
  Label,
  Row,
  Table,
  CustomInput,
} from 'reactstrap';
import IntlMessages from 'helpers/IntlMessages';
import { Colxx } from 'components/common/CustomBootstrap';
import { connect } from 'react-redux';
import { getMachineID } from 'helpers/Utils';
import {
  bindProductDropdowns,
  bindProductGrid,
  dispenseNow,
  DisepnseResetState,
  dispenseSuccess,
  // GetDispanseDataSuccess,
  GetDispanseData,
  DispenseLoadingLoader,
  CheckCanisterData,
} from 'redux/actions';
import { Formik, Form } from 'formik';
import createNotification from 'helpers/alerts';
import * as Yup from 'yup';
import { useHistory } from 'react-router-dom';
import CanisterEligibleForDispenseModal from './canisterEligibleForDispenseModal';

const DispenseManage = ({
  productDropdowns,
  bindProductDropdownsAction,
  bindProductGridAction,
  productGrid,
  dispenseNowAction,
  isSucessfullyAdd,
  resultMessage,
  ResetStateAction,
  loading,
  isLoadingProductGrid,
  isSignalRSuccess,
  dispense_success_data,
  dispenseSuccessAction,
  GetDispanseDataAction,
  dispenseSuccessIsSucessfullyAdd,
  dispenseSuccessResultMessage,
  isSignalRError,
  DispenseLoadingLoaderAction,
  GetDispanseDataSuccess,
  CheckCanisterData,
  CheckCanisterDataAction,
  CheckCanisterDataSuccess,
}) => {
  const [productNamedropdownItem, setProductNamedropdownItem] = useState([]);
  const [productCodedropdownItem, setProductCodedropdownItem] = useState([]);
  const [productCollectiondropdownItem, setproductCollectiondropdownItem] =
    useState([]);
  const [canSizedropdownItem, setcanSizedropdownItem] = useState([]);
  const [unitdropdownItem, setunitdropdownItem] = useState([]);
  const [selectedProductCode, setSelectedProductCode] = useState('');
  const [selectedProductName, setSelectedProductName] = useState('');
  const [selectedCollection, setSelectedCollection] = useState('');
  const [selectedProduct, setSelectedProduct] = useState(0);
  const [selectedProductDispenseAmount, setSelectedProductDispenseAmount] =
    useState('');
  const [isEligibleDispenseModalOpen, seIsEligibleDispenseModalOpen] =
    useState(false);
  const [eligibleForDispenseMessage, setEligibleForDispenseMessage] =
    useState(null);
  const [formValues, setFormValues] = useState();
  // const [formAction, setFormAction] = useState();
  const [notExistedCanister, setNotExistedCanister] = useState(null);

  const DispenseValidationSchema = Yup.object().shape({
    DispenseNumber: Yup.string().required('A dispense number is required!'),
    DispenseUnit: Yup.string().required('A select option is required!'),
  });

  useEffect(() => {
    bindProductDropdownsAction();
  }, []);
  useEffect(async () => {
    if (isSignalRSuccess) {
      let data = dispense_success_data;
      formValues.Amount = data.userID[3];
      await GetDispanseDataAction(productGrid[0].productCode, getMachineID());
      dispenseSuccessAction(formValues, true);
      createNotification('success', 'Success', 'Dispense successfully');
    } else if (isSignalRError) {
      createNotification('error', 'Error', 'Something to wrong ');
      dispenseSuccessAction(formValues, false);
    }
  }, [isSignalRSuccess, isSignalRError]);

  useEffect(async () => {
    console.log('productGrid', productGrid);

    if (productGrid && productGrid[0] && productGrid[0].dispenseAmount) {
      let rtd = await CheckCanisterDataAction(
        productGrid[0].colorCode,
        getMachineID()
      );
      setSelectedProductName(productGrid[0].productName);
      setSelectedCollection(productGrid[0].collection);
      setSelectedProductDispenseAmount(productGrid[0].dispenseAmount);
    }
  }, [productGrid]);

  useEffect(() => {
    if (
      dispenseSuccessIsSucessfullyAdd != null &&
      dispenseSuccessIsSucessfullyAdd > 0
    ) {
      // createNotification('success', 'Success', dispenseSuccessResultMessage);
      ResetStateAction();
      setSelectedProductCode('');
      setSelectedProductName('');
      setSelectedCollection('');
      setSelectedProduct(0);
      // setEligibleForDispenseMessage(null);
      // seIsEligibleDispenseModalOpen(false);

      document.getElementById('btnDispense').setAttribute('disabled', true);
      document.getElementById('btnDispense').className = 'btn btn-primary';
    } else if (isSucessfullyAdd != null && isSucessfullyAdd === 0) {
      createNotification('error', 'Error', resultMessage);
      DispenseLoadingLoaderAction();
      // setEligibleForDispenseMessage(null);
      // seIsEligibleDispenseModalOpen(false);
    } else if (isSucessfullyAdd != null && isSucessfullyAdd === -1) {
      setEligibleForDispenseMessage(resultMessage);
      seIsEligibleDispenseModalOpen(!isEligibleDispenseModalOpen);
    } else if (
      dispenseSuccessIsSucessfullyAdd != null &&
      dispenseSuccessIsSucessfullyAdd === 0
    ) {
      createNotification('error', 'Error', dispenseSuccessResultMessage);
    }
  }, [isSucessfullyAdd, dispenseSuccessIsSucessfullyAdd]);

  useEffect(async () => {
    if (
      productDropdowns != null &&
      productDropdowns.productNameList != null &&
      productDropdowns.productNameList.length > 0
    ) {
      const productNameOption = productDropdowns.productNameList.map((p) => ({
        value: p.productName,
        label: p.productName,
      }));
      const productCodeOption = productDropdowns.productCodeList.map((p) => ({
        value: p.productCode,
        label: p.productCode,
      }));
      const productCollectionOption = productDropdowns.collectionList.map(
        (c) => ({
          value: c.collection,
          label: c.collection,
        })
      );

      const canSizeOption = productDropdowns.canSize.map((c) => ({
        value: c.size,
        label: c.size,
      }));

      const unitOption = productDropdowns.unitList.map((u) => ({
        value: u.id,
        label: u.name,
      }));

      setProductNamedropdownItem(productNameOption);
      setProductCodedropdownItem(productCodeOption);
      setproductCollectiondropdownItem(productCollectionOption);
      setcanSizedropdownItem(canSizeOption);
      setunitdropdownItem(unitOption);
    }
  }, [productDropdowns]);

  useEffect(() => {
    if (CheckCanisterDataSuccess) {
      setNotExistedCanister(CheckCanisterDataSuccess);
    } else {
      setNotExistedCanister(null);
    }
  }, [CheckCanisterDataSuccess]);

  useEffect(() => {
    const param = {
      ProductCode: selectedProductCode,
      Collection: selectedCollection,
      ProductName: selectedProductName,
    };
    if (
      selectedProductCode !== '' ||
      selectedCollection !== '' ||
      selectedProductName !== ''
    )
      bindProductGridAction(param);
  }, [selectedProductCode, selectedProductName, selectedCollection]);

  /* eslint-disable no-param-reassign */
  function handleCheckboxEvent(checkbox, productID) {
    const gridCheckbox = document.getElementsByName('chkProduct');
    Array.prototype.forEach.call(gridCheckbox, function (el) {
      el.checked = false;
    });
    checkbox.currentTarget.checked = true;
    setSelectedProduct(productID);
    document.getElementById('btnDispense').removeAttribute('disabled');
    document.getElementById('btnDispense').className = 'btn btn-primary';
  }

  function unCheckCheckboxOnDropdown() {
    const gridCheckbox = document.getElementsByName('chkProduct');
    Array.prototype.forEach.call(gridCheckbox, function (el) {
      el.checked = false;
    });
    setSelectedProduct(0);
  }

  const history = useHistory();
  const navigateToHistory = () => {
    history.push('/app/dispense/dispenseHistory');
  };
  useEffect(() => {
    document.getElementById('machineDropdown').style.display = 'block';
  }, []);

  const onDispenseSubmit = (values, actions) => {
    if (notExistedCanister != null && notExistedCanister != undefined) {
      createNotification(
        'error',
        'Error',
        'Please Add this " ' +
          notExistedCanister +
          ' " canisters in to system first'
      );
    } else {
      values.ProductCode = selectedProductCode;
      values.Collection = selectedCollection;
      values.ProductName = selectedProductName;

      const selectedProductToPost = productGrid.filter(
        (x) => x.id === selectedProduct
      );
      values.ColorCode = selectedProductToPost[0].colorCode.trim();
      values.Amount = selectedProductToPost[0].amount.trim();

      // if (values.DispenseUnit == 2) {
      //   values.CanSize = (values.CanSize / 1000).toFixed(4);
      // }
      values.DispenseAmount = selectedProductDispenseAmount;
      values.TotalDispenseAmount = selectedProductDispenseAmount || 1;

      setFormValues(values);
      dispenseNowAction(values);
      actions.resetForm({
        values: {
          ProductCode: '',
          Collection: '',
          ProductName: '',
          CanSize: '',
          DispenseUnit: '',
          ColorCode: '',
          Amount: '',
          DispenseNumber: '',
          TotalDispenseAmount: 1,
        },
      });
    }
  };

  const calculateAmount = (amountStr) => {
    if (amountStr) {
      let amountsArray = amountStr
        .replace(/\s+/g, '')
        .split('-')
        .filter(Boolean);

      // Convert the strings to numbers and sum them up
      let total = amountsArray.reduce((acc, curr) => acc + parseFloat(curr), 0);
      return total;
    } else return 0;
  };

  /* eslint-enable no-param-reassign */

  return (
    <>
      {loading && (
        <div className="customOverlay">
          <div className="loading" />
        </div>
      )}
      <CanisterEligibleForDispenseModal
        eligibleForDispenseMessage={eligibleForDispenseMessage}
        isEligibleDispenseModalOpen={isEligibleDispenseModalOpen}
        toggleModal={() => {
          seIsEligibleDispenseModalOpen(!isEligibleDispenseModalOpen);
          ResetStateAction();
          setSelectedProductCode('');
          setSelectedProductName('');
          setSelectedCollection('');
          setSelectedProduct(0);
        }}
      />

      <Row>
        <Colxx xxs="6">
          <h1>
            <IntlMessages id="forms.page-title" />
          </h1>
          {/* <Separator className="mb-5" /> */}
        </Colxx>
      </Row>
      <Row className="mb-4">
        <Colxx xxs="12">
          <Card>
            <CardBody>
              <CardTitle>
                <Button
                  color="primary"
                  className="mb-2"
                  onClick={navigateToHistory}
                >
                  <IntlMessages id="dispenseHistory-title" />
                </Button>{' '}
              </CardTitle>
              <Formik
                initialValues={{
                  ProductCode: '',
                  Collection: '',
                  ProductName: '',
                  CanSize: '',
                  DispenseUnit: '',
                  ColorCode: '',
                  Amount: '',
                  DispenseAmount: '',
                  DispenseNumber: '',
                  TotalDispenseAmount: '',
                }}
                validationSchema={DispenseValidationSchema}
                onSubmit={onDispenseSubmit}
              >
                {({ handleBlur, handleChange, values, errors }) => (
                  <Form>
                    <FormGroup>
                      <input
                        name="DispenseAmount"
                        id="DispenseAmount"
                        key={Math.random()}
                        value={
                          productGrid &&
                          productGrid[0] &&
                          productGrid[0].dispenseAmount
                        }
                        // type="hidden"
                        style={{ visibility: 'hidden' }}
                      ></input>
                    </FormGroup>

                    <FormGroup row>
                      <Colxx sm={6}>
                        <FormGroup>
                          <Label for="exampleAddressGrid">
                            <IntlMessages id="forms.product-code" />
                          </Label>
                          <select
                            name="ProductCode"
                            id="ddlProductCode"
                            className="form-control"
                            value={selectedProductCode}
                            onChange={(e) => {
                              setSelectedProductCode(e.target.value);
                              unCheckCheckboxOnDropdown();
                            }}
                            onBlur={handleBlur}
                          >
                            <option value="">Select an option..</option>
                            {productCodedropdownItem.map((e) => {
                              return (
                                <option value={e.value} key={Math.random()}>
                                  {e.label}
                                </option>
                              );
                            })}
                          </select>
                        </FormGroup>
                      </Colxx>

                      {/* <Colxx sm={6}>
                        <FormGroup>
                          <Label for="examplePasswordGrid">
                            <IntlMessages id="forms.collection" />
                          </Label>
                          <select
                            name="selectedCollection"
                            id="ddlCollection"
                            className="form-control"
                            value={selectedCollection}
                            onChange={(e) => {
                              setSelectedCollection(e.target.value);
                              unCheckCheckboxOnDropdown();
                            }}
                            onBlur={handleBlur}
                          >
                            <option value="">Select an option..</option>
                            {productCollectiondropdownItem.map((e) => {
                              return (
                                <option value={e.value} key={Math.random()}>
                                  {e.label}
                                </option>
                              );
                            })}
                          </select>
                        </FormGroup>
                      </Colxx> */}

                      {/* <Colxx sm={12}>
                        <FormGroup>
                          <Label for="exampleEmailGrid">
                            <IntlMessages id="forms.product-name" />
                          </Label>
                          <select
                            name="selectedProductName"
                            id="ddlProductName"
                            className="form-control"
                            value={selectedProductName}
                            onChange={(e) => {
                              setSelectedProductName(e.target.value);
                              unCheckCheckboxOnDropdown();
                            }}
                            onBlur={handleBlur}
                          >
                            <option value="">Select an option..</option>
                            {productNamedropdownItem.map((e) => {
                              return (
                                <option value={e.value} key={Math.random()}>
                                  {e.label}
                                </option>
                              );
                            })}
                          </select>
                        </FormGroup>
                      </Colxx> */}

                      <Colxx sm={12}>
                        <Table bordered className="mt-4 mb-5" id="tblProducts">
                          <thead>
                            <tr>
                              <th>Tailoring Code</th>
                              <th>Component Name</th>
                              <th>Vol.</th>
                              <th>Total Vol.</th>
                              <th>Select</th>
                            </tr>
                          </thead>
                          <tbody>
                            {isLoadingProductGrid && (
                              <div className="customOverlay">
                                <div className="loading" />
                              </div>
                            )}
                            {productGrid != null && productGrid.length > 0 ? (
                              productGrid.map((data) => {
                                return (
                                  <>
                                    <tr>
                                      <td>{data.productCode}</td>
                                      <td>{data.colorCode}</td>
                                      <td>{data.amount}</td>
                                      <td>{calculateAmount(data.amount)}</td>
                                      <td>
                                        <CustomInput
                                          type="checkbox"
                                          id="chkProduct"
                                          name="chkProduct"
                                          onChange={(e) => {
                                            handleCheckboxEvent(e, data.id);
                                          }}
                                          inline
                                        />
                                      </td>
                                    </tr>
                                  </>
                                );
                              })
                            ) : (
                              <tr>
                                <td colSpan={6} className="text-center">
                                  No records found...
                                </td>
                              </tr>
                            )}
                          </tbody>
                        </Table>
                      </Colxx>

                      <Colxx sm={6}>
                        <FormGroup>
                          <Label for="exampleEmailGrid">
                            <IntlMessages id="forms.canSize" />
                          </Label>
                          <input
                            name="DispenseNumber"
                            className="form-control"
                            id="DispenseNumber"
                            onChange={handleChange}
                            value={values.DispenseNumber}
                            onBlur={handleBlur}
                            type="number"
                          >
                            {/* <option value="">Select an option..</option>
                            {canSizedropdownItem.map((e) => {
                              return (
                                <option value={e.value} key={Math.random()}>
                                  {e.label}
                                </option>
                              );
                            })} */}
                          </input>
                          {/* {errors.CanSize ? (
                            <div className="invalid-feedback d-block">
                              {errors.CanSize}
                            </div>
                          ) : null} */}
                        </FormGroup>
                      </Colxx>

                      {/* <Colxx sm={6}>
                        <FormGroup>
                          <Label for="exampleEmailGrid">
                            <IntlMessages id="forms.canSize" />
                          </Label>
                          <select
                            name="CanSize"
                            className="form-control"
                            id="ddlCanSize"
                            onChange={handleChange}
                            value={values.CanSize}
                            onBlur={handleBlur}
                          >
                            <option value="">Select an option..</option>
                            {canSizedropdownItem.map((e) => {
                              return (
                                <option value={e.value} key={Math.random()}>
                                  {e.label}
                                </option>
                              );
                            })}
                          </select>
                          {errors.CanSize ? (
                            <div className="invalid-feedback d-block">
                              {errors.CanSize}
                            </div>
                          ) : null}
                        </FormGroup>
                      </Colxx> */}

                      <Colxx sm={6}>
                        <FormGroup>
                          <Label>
                            <IntlMessages id="forms.dispenseUnit" />
                          </Label>
                          <select
                            id="ddlDispenseUnit"
                            className="form-control"
                            name="DispenseUnit"
                            onChange={handleChange}
                            value={values.DispenseUnit}
                            onBlur={handleBlur}
                          >
                            <option value="">Select an option..</option>
                            {unitdropdownItem.map((e) => {
                              return (
                                <option value={e.value} key={Math.random()}>
                                  {e.label}
                                </option>
                              );
                            })}
                          </select>
                          {errors.DispenseUnit ? (
                            <div className="invalid-feedback d-block">
                              {errors.DispenseUnit}
                            </div>
                          ) : null}
                        </FormGroup>
                      </Colxx>

                      <Colxx sm={6}>
                        <FormGroup>
                          <Label for="exampleEmailGrid">
                            <IntlMessages id="forms.total-amount-of-bottle" />
                          </Label>
                          <input
                            name="TotalDispenseAmount"
                            className="form-control"
                            id="TotalDispenseAmount"
                            onChange={handleChange}
                            value={values.TotalDispenseAmount || 1}
                            onBlur={handleBlur}
                            type="number"
                          >
                            {/* <option value="">Select an option..</option>
                            {canSizedropdownItem.map((e) => {
                              return (
                                <option value={e.value} key={Math.random()}>
                                  {e.label}
                                </option>
                              );
                            })} */}
                          </input>
                          {/* {errors.CanSize ? (
                            <div className="invalid-feedback d-block">
                              {errors.CanSize}
                            </div>
                          ) : null} */}
                        </FormGroup>
                      </Colxx>
                    </FormGroup>

                    <Button
                      color="primary"
                      id="btnDispense"
                      disabled="disabled"
                    >
                      <IntlMessages id="forms.dispenseNow" />
                    </Button>
                  </Form>
                )}
              </Formik>
            </CardBody>
          </Card>
        </Colxx>
      </Row>
    </>
  );
};

const mapStateToProps = ({ dispense }) => {
  const {
    productDropdowns,
    productGrid,
    isSucessfullyAdd,
    resultMessage,
    loading,
    isLoadingProductGrid,
    isSignalRSuccess,
    dispense_success_data,
    dispenseSuccessIsSucessfullyAdd,
    GetDispanseDataSuccess,
    dispenseSuccessResultMessage,
    isSignalRError,
    CheckCanisterDataSuccess,
  } = dispense;
  return {
    productDropdowns,
    productGrid,
    isSucessfullyAdd,
    resultMessage,
    loading,
    isLoadingProductGrid,
    isSignalRSuccess,
    dispense_success_data,
    dispenseSuccessIsSucessfullyAdd,
    GetDispanseDataSuccess,
    CheckCanisterDataSuccess,
    dispenseSuccessResultMessage,
    isSignalRError,
  };
};

export default connect(mapStateToProps, {
  bindProductDropdownsAction: bindProductDropdowns,
  bindProductGridAction: bindProductGrid,
  dispenseNowAction: dispenseNow,
  ResetStateAction: DisepnseResetState,
  dispenseSuccessAction: dispenseSuccess,
  GetDispanseDataAction: GetDispanseData,
  // GetDispanseDataSuccessAction:GetDispanseDataSuccess,
  DispenseLoadingLoaderAction: DispenseLoadingLoader,
  CheckCanisterDataAction: CheckCanisterData,
})(DispenseManage);
