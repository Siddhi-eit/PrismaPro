import React, { useState } from 'react';
import { Row } from 'reactstrap';
import IntlMessages from 'helpers/IntlMessages';
import { Colxx, Separator } from 'components/common/CustomBootstrap';
import Breadcrumb from 'containers/navs/Breadcrumb';
import Picker from 'react-scrollable-picker';

const BlankPage = ({ match }) => {
  const [optionGroups] = useState({
    title: ['Mr.', 'Mrs.', 'Ms.', 'Dr.'],
  });
  const [valueGroups, setValueGroups] = useState({ title: 'Mr.' });

  const onhandleChange = (name, value) => {
    setValueGroups({ valueGroups, [name]: value });
  };

  return (
    <>
      <Row>
        <Colxx xxs="12">
          <Breadcrumb heading="menu.blank-page" match={match} />
          <Separator className="mb-5" />
        </Colxx>
      </Row>
      <Row>
        <Colxx xxs="12" className="mb-4">
          <p>
            <IntlMessages id="menu.blank-page" />
          </p>
          <div className="picker-inline-container">
            {/* <Picker
              optionGroups={optionGroups}
              valueGroups={valueGroups}
              onChange={() => {
                onhandleChange();
              }}
            /> */}
          </div>
          <div className="example-container">
            <div className="weui_cells_title">1. As an inline component</div>
            <div className="weui_cells">
              <div className="weui_cell">
                <div className="weui_cell_bd weui_cell_primary">
                  Hi, {valueGroups.title} {valueGroups.firstName}{' '}
                  {valueGroups.secondName}
                </div>
              </div>
            </div>
            <div className="picker-inline-container">
              <Picker
                optionGroups={optionGroups}
                valueGroups={valueGroups}
                onChange={onhandleChange}
              />
            </div>
          </div>
        </Colxx>
      </Row>
    </>
  );
};

export default BlankPage;
