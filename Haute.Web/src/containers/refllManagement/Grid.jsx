import React from 'react';
import { Card, Row } from 'reactstrap';
import ContextMenuContainer from 'containers/pages/ContextMenuContainer';
import Pagination from 'containers/pages/Pagination';
import GridThumbListView from '../pages/refillManagement/GridThumbListView';
import { Colxx } from 'components/common/CustomBootstrap';

function collect(props) {
  return { data: props.data };
}

const Grid = ({
  items,
  allRefillItems,
  displayMode,
  selectedItems,
  onCheckItem,
  currentPage,
  totalPage,
  onContextMenuClick,
  onContextMenu,
  onChangePage,
  onDownload,
}) => {
  return (
    <Row>
      <Colxx xxs="12">
        <Card>
          <div className="pl-2 d-flex flex-grow-1 min-width-zero">
            <div className="card-body align-self-center d-flex flex-column flex-lg-row justify-content-between min-width-zero align-items-lg-center">
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold text-center">
                Canister Code
              </p>
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold text-center">
                Fusion Lab No
              </p>
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold text-center">
                Date Filled
              </p>
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold text-center">
                Lot Nr.
              </p>
              <p className="w-20 list-item-heading mb-1 truncate font-weight-bold text-center">
                Name
              </p>
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold text-center">
                Quantity
              </p>
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold text-center">
                Created Date
              </p>
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold text-center">
                Is Refilled
              </p>
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold text-center">
                Is Active ?
              </p>
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold text-center">
                Download
              </p>
            </div>
          </div>
        </Card>
      </Colxx>
      {items.map((refill) => {
        if (displayMode === 'thumblist') {
          return (
            <GridThumbListView
              key={refill.id}
              refill={refill}
              isSelect={selectedItems.includes(refill.id)}
              collect={collect}
              onCheckItem={onCheckItem}
              onDownloadClick={onDownload}
            />
          );
        }
        return <></>;
      })}
      {allRefillItems === null ||
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

export default React.memo(Grid);
