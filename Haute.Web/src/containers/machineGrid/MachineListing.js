import React from 'react';
import { Button, Card, Col, Row } from 'reactstrap';
import ContextMenuContainer from 'containers/pages/ContextMenuContainer';
import Pagination from 'containers/pages/Pagination';
import MachineThumbListView from 'containers/pages/MachineThumbListView';
import { Colxx } from 'components/common/CustomBootstrap';

function collect(props) {
  return { data: props.data };
}

const MachineListing = ({
  items,
  allMachineItems,
  displayMode,
  selectedItems,
  onCheckItem,
  currentPage,
  totalPage,
  onContextMenuClick,
  onContextMenu,
  onChangePage,
}) => {
  return (
    <Row>
      <Colxx xxs="12">
        <Card>
          <div className="pl-2 d-flex flex-grow-1 min-width-zero">
            <div className="card-body align-self-center d-flex flex-column flex-lg-row justify-content-between min-width-zero align-items-lg-center">
              <p className="w-15 list-item-heading mb-1 truncate font-weight-bold">
                Machine Reg No
              </p>
              <p className="w-15 list-item-heading mb-1 truncate font-weight-bold">
                Shop Name
              </p>
              <p className="w-15 list-item-heading mb-1 truncate font-weight-bold">
                Shop Address
              </p>
              <p className="w-15 list-item-heading mb-1 truncate font-weight-bold">
                City
              </p>
              <p className="w-15 list-item-heading mb-1 truncate font-weight-bold">
                State
              </p>
              <p className="w-15 list-item-heading mb-1 truncate font-weight-bold">
                Mac Address
              </p>
              <p className="w-15 list-item-heading mb-1 truncate font-weight-bold">
                Created Date
              </p>
              <p className="w-15 list-item-heading mb-1 truncate font-weight-bold">
                Is Active ?
              </p>
            </div>
          </div>
        </Card>
      </Colxx>
      {items.map((product) => {
        if (displayMode === 'thumblist') {
          return (
            <MachineThumbListView
              key={product.ID}
              machine={product}
              isSelect={selectedItems.includes(product.ID)}
              collect={collect}
              onCheckItem={onCheckItem}
            />
          );
        }
        return <></>;
      })}
      {allMachineItems === null ||
        (items.length === 0 && (
          <h3 className="mb-3 w-100 text-center">No Data Found</h3>
        ))}
      <Pagination
        currentPage={currentPage}
        totalPage={totalPage}
        onChangePage={(i) => onChangePage(i)}
      />
      <ContextMenuContainer
        onContextMenuClick={onContextMenuClick}
        onContextMenu={onContextMenu}
      />
    </Row>
  );
};

export default React.memo(MachineListing);
